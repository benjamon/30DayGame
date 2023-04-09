using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bentendo.TTS
{
	public class BattleContext
	{
		public BattleRunner battleRunner { get; private set; }
		public BattleAnimator battleAnim { get; private set; }
		public List<Entity> leftEnts;
		public List<Entity> rightEnts;

		public BattleContext(BattleAnimator anim, BattleRunner battleRunner, Entity[] leftEnts, Entity[] rightEnts)
        {
			this.leftEnts = new List<Entity>(leftEnts);
			this.rightEnts = new List<Entity>(rightEnts);
			this.battleAnim = anim;
			this.battleRunner = battleRunner;
			anim.Setup(this);
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
