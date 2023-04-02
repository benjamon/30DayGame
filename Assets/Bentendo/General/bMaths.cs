using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo
{
	public class bMaths
	{
		public static void LookAt2D(Transform t, Vector2 p2)
        {
			var p1 = (Vector2)t.transform.position;
			t.transform.rotation = Quaternion.identity;
			t.Rotate(Vector3.forward, Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * Mathf.Rad2Deg);
        }

        public static void LookUpAt2D(Transform t, Vector3 p2)
		{
			var p1 = (Vector2)t.transform.position;
			t.transform.rotation = Quaternion.identity;
			t.Rotate(Vector3.forward, Mathf.Atan2(p1.x - p2.x, p2.y - p1.y) * Mathf.Rad2Deg);
		}

		public enum LerpStyle
		{
			Bicubic, 
			Bounce,
			Spring,
			Linear,
			Quadratic
		}

		public static float LerpValue(float normalized, LerpStyle style)
		{
			switch (style)
			{
				case LerpStyle.Bicubic:
					return Bicubic(normalized);
				case LerpStyle.Bounce:
					return Bounce(normalized);
				case LerpStyle.Spring:
					return Spring(normalized);
				case LerpStyle.Linear:
					return Linear(normalized);
				case LerpStyle.Quadratic:
					return Quadratic(normalized);
				default:
					return 0;
			}
		}
		static float Bicubic(float normalized)
		{
			return 3 * Mathf.Pow(normalized, 2) - 2 * Mathf.Pow(normalized, 3);
		}
		static float Bounce(float normalized)
		{
			return Mathf.Abs(Mathf.Sin(6 * Mathf.PI * normalized) * (1 - normalized));
		}
		static float Spring(float normalized)
		{
			return 1 - Mathf.Cos(normalized * 4.5f * Mathf.PI) * Mathf.Exp(-5 * normalized);
		}
		static float Linear(float normalized)
		{
			return normalized;
		}
		static float Quadratic(float normalized)
		{
			return Mathf.Pow(normalized, 2);
		}
    }
}
