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
    }
}
