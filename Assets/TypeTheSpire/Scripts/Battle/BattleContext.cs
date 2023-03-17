using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bentendo.TTS
{
	public class BattleContext
	{
		public BattleRunner runner;
		public Entity[] player;
		public Entity[] opponent;

		public BattleContext(BattleRunner runner)
        {
			this.runner = runner;
        }
		
		//entity[] player
		//entity[] enemies
		//timeline
		//battle data (ie cards played, letters typed, words typed, damage taken, time)
	}
	
	public class BattleEvents
    {
		public EntityEvent EntityBorn = new EntityEvent();
		public EntityEvent EntityKilled = new EntityEvent();
		//hurt?
		//healed?
	}

	public class EntityEvent : UnityEvent<Entity> { }
}
