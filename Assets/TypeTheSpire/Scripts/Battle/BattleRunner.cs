using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class BattleRunner : MonoBehaviour
	{
        public CardDef testCard;
        public BattleAnimator battleAnims;
        public TimelineUI timelineUI;
        public EntityDef testEntity;
        public BattleEventDef enemies;
        public KInput kInput;
        BattleTimeline timeline;
        BattleContext context;
        Entity testHero;

        private void Awake()
        {
            testHero = new Entity(testEntity);
            var allied = new Entity[] { testHero };
            context = new BattleContext(battleAnims, allied, enemies.ToEntityInstances());
            timeline = new BattleTimeline(this, 10);
            timelineUI.Setup(timeline);
        }

        private void Update()
        {
            timeline.AddTime(Time.deltaTime);
        }

        public void PlayCard(Entity caster, Card card, int timeUntil)
        {
			timeline.EnqueueAction(() => card.def.Cast(context, caster, card), timeUntil);
		}
	}
}
