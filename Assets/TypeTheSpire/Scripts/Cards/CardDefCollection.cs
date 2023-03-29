using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
    [CreateAssetMenu(fileName = "new Card Deck", menuName = "new TTS Card Deck", order = 0)]
    public class CardDefCollection : ScriptableObject
	{
		public CardDef[] cardDefs;

        internal List<Card> GetCards()
        {
            List<Card> result = new List<Card>();
            for (int i = 0; i < cardDefs.Length; i++)
                result.Add(new Card(cardDefs[i]))
            return result;
        }
    }
}
