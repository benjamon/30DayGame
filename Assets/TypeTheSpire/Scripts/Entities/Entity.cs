using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class Entity
	{
		public IEntityProvider Source { get; private set; }
		public int MaxHP { get; private set; }
		public int HP { get; private set; }
		//statuses
		public Deck<Card> Deck { get; private set; }
		public EntityBody Body { get; private set; }

		public Entity(IEntityProvider provider, EntityBody body)
        {
			this.Source = provider;
			MaxHP = provider.GetMaxHP();
			HP = provider.GetHP();
			Deck = new Deck<Card>(provider.GetCards());
        }
	}
}
