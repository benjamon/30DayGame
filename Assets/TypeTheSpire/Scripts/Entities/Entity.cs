using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class Entity
	{
		public IEntityProvider source { get; private set; }
		public int maxHP { get; private set; }
		public int HP { get; private set; }
		//statuses
		public Deck<Card> deck { get; private set; }

		public Entity(IEntityProvider provider)
        {
			this.source = provider;
			maxHP = provider.GetMaxHP();
			HP = provider.GetHP();
			deck = new Deck<Card>(provider.GetCards());
        }
	}
}
