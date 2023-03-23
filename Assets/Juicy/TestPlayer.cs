using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class TestPlayer : MonoBehaviour
	{
		public GameObject bulletPrefab;
        public float bulletSpeed;

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var go = GameObject.Instantiate(bulletPrefab, (Vector2)transform.position, Quaternion.identity, null);
                bMaths.LookUpAt2D(go.transform, target);
                go.GetComponent<TestBullet>().Fire(go.transform.up * bulletSpeed);
            }
        }
    }
}
