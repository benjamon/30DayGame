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
		[Range(-4,4)]
		public int wordLengthModifier;
		public GameObject hitEffect;
		public GameObject cardPrefab;
		public CardAction[] actions;

        internal IEnumerator Cast(BattleContext context, CastInfo info)
        {
			for (int i = 0; i < actions.Length; i++)
				yield return context.battleAnim.StartCoroutine(actions[i].Cast(context, info));
        }
    }

	[System.Serializable]
	public class CardAction
	{
		public ActionType actionId;
		public ActionTarget targetId;
		public StatusEffect statusType;
		public StatID targetStat;
		public string animationId;
		public bool doNotAnimate;

		//general card reference to be used in different ways depending on need,
		//maybe you could "draw shivs", play a separate set of actions if you kill,
		//or get a temporary card reward on kill... this is all hypothetical though.
		public CardDef cardAction;
		//public bool replaceWithCardOnKill;
		//public bool playCardOnKill;
		
		public int amount;
		
		public IEnumerator Cast(BattleContext context, CastInfo info)
        {
			var caster = info.source;
			var card = info.card;
			var castLock = new CastLock();
			if (doNotAnimate)
			{
				Apply(context, info);
				yield break;
			}
			Action action = () => {
				castLock.applied = true;
				Apply(context, info);
			};
			yield return context.battleAnim.Animate(this, caster, action, context.rightEnts[0].Body.transform, card.def.hitEffect);
			if (!castLock.applied)
				action.Invoke();
		}

		void Apply(BattleContext context, CastInfo info)
        {
			var target = GetTarget(context, info);
            switch (actionId)
            {
                case ActionType.PLAY_CARD:
                    break;
                case ActionType.STRIKE:
					target.ApplyDamage(new Damage(info.source, amount));
                    break;
                case ActionType.BLOCK:
					target.ApplyArmor(amount);
                    break;
                case ActionType.HEAL:
					target.AddHealth(amount);
                    break;
                case ActionType.APPLY_STATUS:
                    break;
                case ActionType.DRAW:
                    break;
                case ActionType.DISCARD:
                    break;
				case ActionType.MODIFY_STAT:
					target.Stats.GetStat(targetStat).Value += amount;
					break;
            }
		}

		public Entity GetTarget(BattleContext context, CastInfo info)
        {
			if (targetId == ActionTarget.SELF)
				return info.source;
			if (info.source.isLeft)
				return context.rightEnts[0];
			else
				return context.leftEnts[0];
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
		public Entity source;
		public Card card;
		public string word;
		public float timeToType;
		public int typos;
		public CastInfo(Entity src, Card crd)
        {
			source = src;
			card = crd;
        }
		public CastInfo() { }
    }
}
