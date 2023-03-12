using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class CardDefCollection
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
