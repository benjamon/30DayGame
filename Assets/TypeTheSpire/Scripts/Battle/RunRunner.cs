using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bentendo.TTS
{
	public class RunRunner : MonoBehaviour
	{
        public static RunRunner Instance;

        public EntityDef TempPlayerDef;
        public RunGenerationSpec TempRunSpec;

        public PlayerState PlayerState;
        public EncounterNode Current;
        [HideInInspector]
        public int BaseWordLength;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
#if UNITY_EDITOR
            BaseWordLength = 7;
#else
            BaseWordLength = 5;
#endif
        }

        public void StartRun()
        {
            CreateRun(TempPlayerDef, TempRunSpec);
            SceneManager.LoadScene("TTS");
        }

        //new run
        public void CreateRun(EntityDef playerDef, RunGenerationSpec spec)
        {
			PlayerState = new PlayerState(playerDef);
            Current = spec.Generate();
        }

        public bool SelectNextBattle()
        {
            if (Current.NextNodes.Count == 0)
            {
                Current = null;
                return false;
            }

            Current = Current.NextNodes[0];
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
