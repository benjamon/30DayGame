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

        public bool SelectNextBattle()
        {
            if (current.NextNodes.Count == 0)
            {
                current = null;
                return false;
            }

            current = current.NextNodes[0];
            return true;
        }

        public void StartNewGame()
        {
            Instance = null;
            SceneManager.LoadScene("TTS_Start");
            GameObject.Destroy(gameObject);
        }
	}
}
