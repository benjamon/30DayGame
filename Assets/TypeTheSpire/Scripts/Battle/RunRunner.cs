using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class RunRunner
	{
		public PlayerState playerState;
		

		public RunRunner(IEntityProvider playerDef, RunGenerationSpec spec)
        {
			playerState = new PlayerState(playerDef);
        }
	}
}
