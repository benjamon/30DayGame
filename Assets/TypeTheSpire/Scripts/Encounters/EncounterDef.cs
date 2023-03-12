using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	[CreateAssetMenu(fileName = "new encounter def", menuName = "new TTS encounter def", order = 5)]
	public class EncounterDef : ScriptableObject
	{
		public BattleEventDef[] events;
	}
}
