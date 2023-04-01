using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class BattleRunner : MonoBehaviour
	{
        public BattleAnimator battleAnims;
        public TimelineUI timelineUI;
        public CardManager cardManager;
        BattleTimeline timeline;
        BattleContext battleContext;
        bool isSetup;

        public void SetupBattle(IEntityProvider player, BattleEventDef battle)
        {
            var p1 = player.GetEntity();
            battleContext = new BattleContext(battleAnims, this, new Entity[] { p1 }, battle.GetEntities());
            timeline = new BattleTimeline(this, 10);
            timelineUI.Setup(timeline);
            cardManager.Setup(battleContext, p1);
            isSetup = true;
        }

        private void Update()
        {
            if (!isSetup)
                return;
            timeline.AddTime(Time.deltaTime);
        }

        public void PlayCard(Entity caster, Card card, int timeUntil)
        {
			timeline.EnqueueAction(() => card.def.Cast(battleContext, caster, card), timeUntil);
		}

        public void PlayCardImminent(Entity caster, Card card)
        {
            PlayCard(caster, card, 0);
        }
	}
}
