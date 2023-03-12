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
		public WordCost[] cost;
		public CardAction[] actions;

        internal void Cast(BattleContext context, Entity self, Card card)
        {
			for (int i = 0; i < actions.Length; i++)
				actions[i].Cast(context, self, card);
        }
        //
        //CardAction[]
    }

	[System.Serializable]
	public class CardAction
	{
		public ActionType actionId;
		public ActionTarget targetId;
		public StatusEffect statusType;
		
		//IF STRIKE
		public bool replaceWithCardOnKill;
		public bool playCardOnKill;

		public CardDef card;
		public int amount;

		public void Cast(BattleContext context, Entity self, Card card)
        {

        }
		//Cast(Context, caster, cardInstance, ?CardActionParent, ?Target)
		//..cardData
	}

	[System.Serializable]
	public class WordCost
	{
		//enum dictId
		public int minLength = 3;
		public int maxLength = 0;
		public int reps = 1;
	}
}
