using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class TestBullet : MonoBehaviour
	{
		Vector2 velocity;

		public void Fire(Vector2 velocity)
        {
			this.velocity = velocity;
        }

        private void FixedUpdate()
        {
            transform.position += (Vector3)velocity * Time.fixedDeltaTime;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
        }
    }
}
