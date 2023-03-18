using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class EntityBody : MonoBehaviour
	{
		Animation Anim;

        private void Awake()
        {
            Anim = GetComponent<Animation>();
        }

        public void PlayAnimation(string animId, Card caster)
        {
            var clip = Anim.GetClip(animId);
            if (clip != null)
            {
                Anim.Stop();
                Anim.clip = Anim.GetClip(animId);
                Anim.Play();
            }
        }
	}
}
