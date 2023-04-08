using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	[CreateAssetMenu(fileName = "new Card Def", menuName = "new TTS Card Def", order = 0)]
	public class CardDef : ScriptableObject
	{
		public string title;
		public Sprite Icon;
		[TextArea(2,4)]
		public string description;
		public WordCost cost;
		public GameObject hitEffect;
		public CardAction[] actions;

        internal IEnumerator Cast(BattleContext context, Entity self, CastInfo info)
        {
			for (int i = 0; i < actions.Length; i++)
				yield return context.battleAnim.StartCoroutine(actions[i].Cast(context, self, info.card));
        }
    }

	[System.Serializable]
	public class CardAction
	{
		public ActionType actionId;
		public ActionTarget targetId;
		public StatusEffect statusType;
		public string animationId;
		public bool doNotAnimate;

		//general card reference to be used in different ways depending on need,
		//maybe you could "draw shivs", play a separate set of actions if you kill,
		//or get a temporary card reward on kill... this is all hypothetical though.
		public CardDef cardAction;
		//public bool replaceWithCardOnKill;
		//public bool playCardOnKill;
		
		public int amount;
		
		public IEnumerator Cast(BattleContext context, Entity caster, Card card)
        {
			var castLock = new CastLock();
			if (doNotAnimate)
			{
				Apply(context, caster, card);
				yield break;
			}
			Action action = () => {
				castLock.applied = true;
				Apply(context, caster, card);
			};
			yield return context.battleAnim.Animate(this, caster, action, context.rightEnts[0].Body.transform, card.def.hitEffect);
			if (!castLock.applied)
				action.Invoke();
		}

		void Apply(BattleContext context, Entity caster, Card card)
        {
			Debug.Log("applied");
        }

		class CastLock
        {
			public bool applied;
        }
	}

	[System.Serializable]
	public class WordCost
	{
		//enum dictId
		public int minLength = 3;
		public int maxLength = 0;
		public int reps = 1;
	}

	public class CastInfo
	{
		public Card card;
		public string word;
		public float timeToType;
		public int typos;
    }
}
