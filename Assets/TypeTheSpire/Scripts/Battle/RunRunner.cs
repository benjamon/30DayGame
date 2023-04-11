using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bentendo.TTS
{
	public class RunRunner : MonoBehaviour
	{
        public static RunRunner Instance;
		public PlayerState playerState;

        public EntityDef tempPlayerDaef;
        public RunGenerationSpec tempRunSpec;

        public EncounterNode current;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public void StartRun()
        {
            CreateRun(tempPlayerDaef, tempRunSpec);
            SceneManager.LoadScene("TTS");
        }

        //new run
        public void CreateRun(EntityDef playerDef, RunGenerationSpec spec)
        {
			playerState = new PlayerState(playerDef);
            current = spec.Generate();
        }

        public void MoveToNextBattle()
        {
            if (current.NextNodes.Count == 0)
            {
                SceneManager.LoadScene("TTS_WIN");
                current = null;
                return;
            }

            current = current.NextNodes[0];
            SceneManager.LoadScene("TTS", LoadSceneMode.Single);
        }
	}
}
