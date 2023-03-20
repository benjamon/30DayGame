using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class EntityBody : MonoBehaviour
	{
        public SpriteRenderer BodyImage;
        public Transform ProceduralRoot;
		Animation anim;
        Action nextAction;

        private void Awake()
        {
            anim = GetComponent<Animation>();
        }

        public void Setup(Entity e)
        {
            BodyImage.sprite = e.Source.GetSprite();
        }

        //we could support animations and programattic stuff by bringing this logic out
        public IEnumerator PlayAnimation(string animId, Action action)
        {
            var clip = anim.GetClip(animId);
            nextAction = action;
            if (clip != null)
            {
                anim.Stop();
                anim.clip = anim.GetClip(animId);
                anim.Play();
                while (anim.isPlaying)
                    yield return null;
            }
            nextAction = null;
        }

        public void ApplyAction()
        {
            if (nextAction != null)
                nextAction.Invoke();
            nextAction = null;
        }
	}
}
