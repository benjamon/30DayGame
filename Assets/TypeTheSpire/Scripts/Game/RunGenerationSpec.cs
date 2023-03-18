using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{

	[CreateAssetMenu(fileName = "new RunGenerationSpec", menuName = "new TTS RunGenerationSpec", order = 0)]
	public class RunGenerationSpec : ScriptableObject
	{
		public LayerDef[] Layers;
	}

	[System.Serializable]
	public class LayerDef
    {
		public EncounterGroupDef[] EncounterOptions;
    }
}
