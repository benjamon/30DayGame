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
        BattleTimeline timeline;
        BattleContext context;
        Entity testHero;
        //StartBattle(PlayerState, EncounterEvent)

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
            if (Input.GetKeyDown(KeyCode.V))
            {
                PlayCard(testHero, new Card(testCard), 30);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                PlayCard(testHero, new Card(testCard), 60);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
                timeline.TimeScale *= 1.1f;
            if (Input.GetKeyDown(KeyCode.DownArrow))
                timeline.TimeScale *= .9f;

            timeline.AddTime(Time.deltaTime);
        }

        public void PlayCard(Entity caster, Card card, int timeUntil)
        {
			timeline.EnqueueAction(() => card.def.Cast(context, caster, card), timeUntil);
		}
	}
}
