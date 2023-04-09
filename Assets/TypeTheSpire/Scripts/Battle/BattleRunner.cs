using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class BattleRunner : MonoBehaviour
	{
        public static int TICKS_PER_SECOND = 1;

        public BattleAnimator battleAnims;
        public TimelineUI timelineUI;
        public CardManager cardManager;
        BattleTimeline timeline;
        BattleContext battleContext;
        bool isSetup;
        public bool isRunning { get; private set; }

        public void SetupBattle(IEntityProvider player, BattleEventDef runner)
        {
            var pstate = new PlayerState(player);
            var p1 = pstate.GetEntity(this);
            battleContext = new BattleContext(battleAnims, this, new Entity[] { p1 }, runner.GetEntities(this));
            timeline = new BattleTimeline(this, TICKS_PER_SECOND);
            timelineUI.Setup(timeline);
            cardManager.Setup(battleContext, p1);
            isSetup = true;
            isRunning = true;
            foreach (var e in battleContext.rightEnts)
                StartCoroutine(PlayEnemyHand(e));
        }

        public IEnumerator PlayEnemyHand(Entity e)
        {
            while (true)
            {
                if (e.Deck.DrawPileEmpty())
                    e.Deck.Shuffle();
                if (!e.Deck.TryDrawNext(out Card card))
                    throw new System.Exception("enemy out of cards");
                PlayCard(new CastInfo(e, card), 5);
                e.Deck.AddToDiscard(card);
                yield return new WaitForSeconds(5f);
            }
        }

        private void Update()
        {
            if (!isSetup || !isRunning)
                return;
            timeline.AddTime(Time.deltaTime);
        }

        public BattleAction PlayCard(CastInfo info, int timeUntil)
        {
			return timeline.EnqueueAction(() => info.card.def.Cast(battleContext, info), timeUntil);
		}

        public void PlayCardImminent(CastInfo info)
        {
            PlayCard(info, 0);
        }
	}
}
