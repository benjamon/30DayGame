using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class Entity
	{
		public int maxHP { get; private set; }
		public int HP { get; private set; }
		//statuses
		public Deck<Card> cards;
	}
}
