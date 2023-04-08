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
            var p1 = player.GetEntity(this);
            battleContext = new BattleContext(battleAnims, this, new Entity[] { p1 }, runner.GetEntities(this));
            timeline = new BattleTimeline(this, TICKS_PER_SECOND);
            timelineUI.Setup(timeline);
            cardManager.Setup(battleContext, p1);
            isSetup = true;
            isRunning = true;
        }

        private void Update()
        {
            if (!isSetup || !isRunning)
                return;
            timeline.AddTime(Time.deltaTime);
        }

        public BattleAction PlayCard(Entity caster, CastInfo info, int timeUntil)
        {
			return timeline.EnqueueAction(() => info.card.def.Cast(battleContext, caster, info), timeUntil);
		}

        public void PlayCardImminent(Entity caster, CastInfo info)
        {
            PlayCard(caster, info, 0);
        }
	}
}
