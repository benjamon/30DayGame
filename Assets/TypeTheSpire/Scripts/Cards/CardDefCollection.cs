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

        internal Card[] GetCards()
        {
            Card[] cards = new Card[cardDefs.Length];
            for (int i = 0; i < cards.Length; i++)
                cards[i] = new Card(cardDefs[i]);
            return cards;
        }
    }
}
