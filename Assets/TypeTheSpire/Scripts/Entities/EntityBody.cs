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

        private void Awake()
        {
            anim = GetComponent<Animation>();
        }

        public void Setup(Entity e)
        {
            BodyImage.sprite = e.Source.GetSprite();
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
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
            var go = GameObject.Instantiate(HitEffect);
            go.transform.position = BodyImage.transform.position;
            Destroy(go, .5f);
        }

        public void StrikeTarget(float duration)
        {
            StartCoroutine(StrikeReturn(duration));
        }

        IEnumerator StrikeReturn(float duration)
        {
            yield return Lerper.LerpTo(target.position, duration);
            ApplyAction();
            yield return Lerper.LerpTo(Vector3.zero, duration*.3f);
        }
	}
}
