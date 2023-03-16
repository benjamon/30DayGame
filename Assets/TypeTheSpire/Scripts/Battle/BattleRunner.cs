using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class BattleRunner : MonoBehaviour
	{
        BattleTimeline timeline = new BattleTimeline();
        BattleContext context = new BattleContext();
        //StartBattle(PlayerState, EncounterEvent)

        public void PlayCard(Entity caster, Card card, int timeUntil)
        {
			//timeline.EnqueueAction(() => card.def.Cast(context, caster, card), timeUntil);
			timeline.EnqueueAction(() => Debug.Log("castu desu ne " + timeUntil), timeUntil);
		}
	}
}
