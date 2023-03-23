using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class ProjectileEffects : MonoBehaviour
	{
		public AutoEffect CreateEffect;
        public AutoEffect TrailEffect;
        public AutoEffect HitEffect;
        public AutoEffect ExpireEffect;
        Transform trail;

        private void Start()
        {
            CreateEffect.Spawn(gameObject);
            trail = TrailEffect.Spawn(gameObject, false).transform;

        }

        private void LateUpdate()
        {
            if (trail == null)
                return;
            trail.position = transform.position;
            trail.rotation = transform.rotation;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HitEffect.Spawn(gameObject);
        }

        public void Kill()
        {
            ExpireEffect.Spawn(gameObject);
        }

        private void OnDestroy()
        {
            if (trail != null)
                GameObject.Destroy(trail.gameObject, TrailEffect.DeathTimer);
        }

        [System.Serializable]
        public class AutoEffect
        {
            public GameObject Prefab;
            public float DeathTimer;

            public GameObject Spawn(GameObject g, bool auto = true)
            {
                if (Prefab)
                {
                    var fx = GameObject.Instantiate(Prefab, g.transform.position, g.transform.rotation, null);
                    if (DeathTimer >= 0f && auto)
                    {
                        GameObject.Destroy(fx, DeathTimer);
                    }
                    return fx;
                }
                return null;
            }
        }
    }
}
