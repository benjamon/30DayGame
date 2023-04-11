using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	[CreateAssetMenu(fileName = "new encounter def", menuName = "new TTS encounter def", order = 5)]
	public class EncounterDef : ScriptableObject
	{
		public List<BattleEventDef> BattleEvents;
		public EncounterEventType EventType;

		public BattleEventDef GetRandom()
        {
			return BattleEvents[Random.Range(0, BattleEvents.Count)];
        }
	}

	public enum EncounterEventType
    {
		Battle
    }
}
