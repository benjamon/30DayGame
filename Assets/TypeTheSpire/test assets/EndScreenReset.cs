using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bentendo.TTS
{
	public class EndScreenReset : MonoBehaviour
	{
		public void NewGame()
        {
			GameObject.Destroy(RunRunner.Instance);
			SceneManager.LoadScene("TTS_Start");
        }
	}
}
