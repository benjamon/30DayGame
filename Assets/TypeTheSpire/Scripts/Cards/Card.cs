using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class Card
	{
		public CardDef def;

		public void Cast(BattleContext context, Entity self)
        {
			def.Cast(context, self, this);
        }
	}
}
