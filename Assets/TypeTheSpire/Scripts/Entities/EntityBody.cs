using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class EntityBody : MonoBehaviour
	{
        public SpriteRenderer BodyImage;
        public GameObject HitEffect;
        public LerpTrolley Lerper;
		Animation anim;
        Action nextAction;
        Transform target;
        GameObject currentEffect;

        private void Awake()
        {
            anim = GetComponent<Animation>();
        }

        public void Setup(Entity e)
        {
            e.Source.GetDef().SetupSprite(BodyImage);
        }

        //we could support animations and programattic stuff by bringing this logic out
        public IEnumerator PlayAnimation(string animId, Action action, Transform target, GameObject hitEffect)
        {
            this.target = target;
            this.currentEffect = hitEffect;
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
            var go = GameObject.Instantiate((currentEffect == null) ? HitEffect : currentEffect);
            go.transform.position = BodyImage.transform.position;
            Destroy(go, .5f);
        }
	}
}
