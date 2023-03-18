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

        private void Start()
        {
			context = new BattleContext(null);
			context.leftEnts = new Entity[]
			{
				new Entity(def),
				new Entity(def),
				new Entity(def),
				new Entity(def),
			};
			context.rightEnts = new Entity[]
			{
				new Entity(def),
				new Entity(def),
				new Entity(def),
				new Entity(def),
				new Entity(def),
			};
			Setup(context);
		}

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
				flip *= -1;
			}
			for (int i = 0, flip = 1; i < context.rightEnts.Length; i++)
			{
				var ent = context.rightEnts[i];
				var go = GameObject.Instantiate(BodyPrefab, RightRoot);
				go.name = go.name.Replace("(Clone)", "");
				var flippedOffset = new Vector2(placementOffset.x, -placementOffset.y);
				if (i != 0)
					go.transform.localPosition = flippedOffset * flip * ((i + 1) / 2);
				var ebod = go.GetComponent<EntityBody>();
				ent.SetBody(ebod);
				flip *= -1;
			}
		}

		public IEnumerator Animate(CardAction action, Entity caster, Action apply)
        {
			if (action.doNotAnimate)
				yield break;
			//we'll want to move this to either some meta animation or timeline scriptable object to play
			//animations involving environmental changes, multiple entities screenshake etc
			var id = (string.IsNullOrWhiteSpace(action.animationId)) ? DefaultAnimation : action.animationId;
			caster.Body.PlayAnimation(DefaultAnimation, apply);
		}
	}
}
