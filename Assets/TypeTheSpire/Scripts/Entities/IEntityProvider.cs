using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public interface IEntityProvider
	{
		Entity GetEntity();
		int GetHP();
		int GetMaxHP();
		List<Card> GetCards();
		Sprite GetSprite();
	}
}
