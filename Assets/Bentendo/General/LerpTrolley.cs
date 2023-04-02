using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Bentendo.bMaths;

namespace Bentendo
{
	public class LerpTrolley : MonoBehaviour
	{
        public AnimationCurve Curve;
        public bool useCurve;

        public UnityEvent OnComplete;

        public LerpStyle Style;
        public bool isLerping { get; private set; }
		Coroutine lastLerp;

        public IEnumerator LerpTo(Vector3 target, float duration)
		{
			if (isLerping)
				StopCoroutine(lastLerp);
			float startTime = Time.time;
			Vector3 startPos = transform.position;
			Vector3 delta = target - transform.position;
			isLerping = true;
			while (Time.time-startTime < duration)
			{
				float norm = (Time.time - startTime) / duration;
				if (useCurve)
					norm = Curve.Evaluate(norm);
				else
					norm = bMaths.LerpValue(norm, Style);
				transform.position = startPos + delta * norm;
				yield return null;
			}
			transform.position = target;
			isLerping = false;
		}
	}
}