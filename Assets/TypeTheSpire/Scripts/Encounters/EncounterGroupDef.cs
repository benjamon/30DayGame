using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
    [CreateAssetMenu(fileName = "new EncounterGroup", menuName = "new TTS EncounterGroup", order = 0)]
	public class EncounterGroupDef : ScriptableObject
	{
		public Sprite Icon;
		public string Title;
		public List<BattleEventDef> Encounters;

        internal BattleEventDef GetRandom()
        {
			return Encounters[Random.Range(0, Encounters.Count)];
        }
    }
}
