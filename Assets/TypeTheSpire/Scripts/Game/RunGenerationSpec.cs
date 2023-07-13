using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bentendo.TTS
{

	[CreateAssetMenu(fileName = "new RunGenerationSpec", menuName = "new TTS RunGenerationSpec", order = 0)]
	public class RunGenerationSpec : ScriptableObject
	{
		public List<EncounterGroupDef> Layers;

		public EncounterNode Generate()
        {
			EncounterNode root = new EncounterNode(Layers[0].GetRandom());
			var crnt = root;
			for (int i = 1; i < Layers.Count; i++)
            {
				var next = new EncounterNode(Layers[i].GetRandom());
				crnt.NextNodes.Add(next);
				crnt = next;
            }
			return root;
        }
	}

	public class EncounterNode 
	{
		public List<EncounterNode> NextNodes = new List<EncounterNode>();
		public BattleEventDef Encounter;
		public EncounterNode(BattleEventDef def)
        {
			Encounter = def;
        }
	}
}
