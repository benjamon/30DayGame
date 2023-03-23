using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo
{
	public class PhysUtils
	{
        [System.Serializable]
		public class bSpring
        {
            public float k = 1.0f;
            public float damp = .9f;
            [Range(.01f,10f)]
            public float mass = 1.0f;

            [System.NonSerialized]
            public float position;
            [System.NonSerialized]
            public float velocity;

            public bool AtRest() => (Mathf.Abs(position) <= .01f && Mathf.Abs(velocity) <= .01f);

            public void Update(float delta)
            {
                float force = -k * position;

                float acceleration = force / mass;

                velocity += acceleration * delta;

                position += velocity * delta;

                velocity *= damp;
            }
        }
	}
}
