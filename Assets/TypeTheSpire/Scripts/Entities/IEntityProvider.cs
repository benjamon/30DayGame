using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public interface IEntityProvider
	{
		Entity GetEntity(BattleRunner runner);
		int GetHP();
		EntityStats GetStats();
		List<Card> GetCards();
		EntityDef GetDef();
	}
}
