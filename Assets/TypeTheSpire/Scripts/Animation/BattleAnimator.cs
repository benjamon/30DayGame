using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class BattleAnimator : MonoBehaviour
	{
		public GameObject BodyPrefab;
		public Transform LeftRoot;
		public Transform RightRoot;
		public string DefaultAnimation;
		public Vector2 placementOffset;

		public EntityDef def;

		BattleContext context;

        public void Setup(BattleContext context)
        {
			this.context = context;

			for (int i = 0, flip = 1; i < context.leftEnts.Length; i++)
            {
				var ent = context.leftEnts[i];
				var go = GameObject.Instantiate(BodyPrefab, LeftRoot);
				go.name = go.name.Replace("(Clone)", "");
				if (i != 0)
					go.transform.localPosition = placementOffset * flip * ((i+1) / 2);
				var ebod = go.GetComponent<EntityBody>();
				ent.SetBody(ebod);
				go.GetComponent<EntityUI>().Setup(ent);
				flip *= -1;
			}

			var flippedOffset = new Vector2(placementOffset.x, -placementOffset.y);
			for (int i = 0, flip = 1; i < context.rightEnts.Length; i++)
			{
				var ent = context.rightEnts[i];
				var go = GameObject.Instantiate(BodyPrefab, RightRoot);
				go.name = go.name.Replace("(Clone)", "");
				if (i != 0)
					go.transform.localPosition = flippedOffset * flip * ((i + 1) / 2);
				var ebod = go.GetComponent<EntityBody>();
				ent.SetBody(ebod);
				go.GetComponent<EntityUI>().Setup(ent);
				flip *= -1;
			}
		}

		public IEnumerator Animate(CardAction action, Entity caster, Action apply, Transform target, GameObject hitEffect)
        {
			if (action.doNotAnimate)
				yield break;
			//we'll want to move this to either some meta animation or timeline scriptable object to play
			//animations involving environmental changes, multiple entities screenshake etc
			var id = (string.IsNullOrWhiteSpace(action.animationId)) ? DefaultAnimation : action.animationId;
			yield return caster.Body.PlayAnimation(id, apply, target, hitEffect);
		}

        private void OnDrawGizmosSelected()
		{
			for (int i = 0, flip = 1; i < 5; i++)
			{
				Gizmos.DrawSphere(LeftRoot.transform.position + (Vector3)placementOffset * flip * ((i + 1) / 2), .1f);
				flip *= -1;
			}
			var flippedOffset = new Vector2(placementOffset.x, -placementOffset.y);
			for (int i = 0, flip = 1; i < 5; i++)
			{
				Gizmos.DrawSphere(RightRoot.transform.position + (Vector3)flippedOffset * flip * ((i + 1) / 2), .1f);
				flip *= -1;
			}
		}
    }
}
