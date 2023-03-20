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
        BattleTimeline timeline;
        BattleContext context;
        //StartBattle(PlayerState, EncounterEvent)

        private void Awake()
        {
            context = new BattleContext(battleAnims);
            timeline = new BattleTimeline(this, 10);
            timelineUI.Setup(timeline);
        }
        int time = 0;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                PlayCard(null, new Card(testCard), 30);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                PlayCard(null, new Card(testCard), 60);
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
