using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class TestLog : MonoBehaviour
	{
		int x;

		public void Rstrt()
        {
			x = 0;
			Debug.Log(x);
        }
		public void Log()
        {
			Debug.Log(++x);
        }
	}
}
