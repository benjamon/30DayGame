using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class RunDef
	{
		public LayerDef[] Layers;
	}

	[System.Serializable]
	public class LayerDef
    {
		public EncounterGroupDef[] EncounterOptions;
    }
}
