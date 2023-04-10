using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class RunRunner
	{
		public PlayerState playerState;
		
		//new run
		public RunRunner(EntityDef playerDef, RunGenerationSpec spec)
        {
			playerState = new PlayerState(playerDef);
        }
	}
}
