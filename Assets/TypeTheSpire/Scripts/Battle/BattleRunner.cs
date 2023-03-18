using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class BattleRunner : MonoBehaviour
	{
        public CardDef testCard;
        public BattleAnimator battleAnims;
        BattleTimeline timeline;
        BattleContext context;
        //StartBattle(PlayerState, EncounterEvent)

        private void Awake()
        {
            timeline = new BattleTimeline(this);
            context = new BattleContext(battleAnims);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                PlayCard(null, new Card(testCard), 1);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                PlayCard(null, new Card(testCard), 0);
            }

            if (Input.GetKeyDown(KeyCode.Space))
                timeline.Tick();
        }

        public void PlayCard(Entity caster, Card card, int timeUntil)
        {
			timeline.EnqueueAction(() => card.def.Cast(context, caster, card), timeUntil);
		}
	}
}
