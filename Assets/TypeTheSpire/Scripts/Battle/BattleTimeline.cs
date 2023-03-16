using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class BattleTimeline
	{
		//maintain
		public OrderedQueue<BattleAction> futureActions = new OrderedQueue<BattleAction>(val => val.time);
		public int Time;

		public void EnqueueAction(Action act, int timeUntil)
        {
			futureActions.Enqueue(new BattleAction
			{
				action = act,
				time = Time + timeUntil,
			});
        }

		public void Tick()
        {
			Time++;
			var next = futureActions.Peek();
			while (next != null && Time >= next.time)
            {
				futureActions.Dequeue();
				next.action.Invoke();
				next = futureActions.Peek();
            }
        }

		public class BattleAction
        {
			public int time;
			public Action action;
        }
	}
}
