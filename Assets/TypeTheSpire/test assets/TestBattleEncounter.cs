using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bentendo.TTS
{
	public class TestBattleEncounter : MonoBehaviour
	{
        public BattleRunner runner;
        public EntityDef testPlayer;
        public BattleEventDef enemies;

        public void Start()
        {
            if (WordProvider.Instance == null)
            {
                Debug.LogError("loading words");
                SceneManager.LoadScene(0);
                return;
            }
            else
            {
                var pstate = new PlayerState(testPlayer);
                runner.SetupBattle(pstate, enemies);
            }
        }
    }
}
