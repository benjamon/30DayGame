using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class BattleTimeline
	{
		public int Time;

		OrderedQueue<BattleAction> futureActions = new OrderedQueue<BattleAction>(val => val.time);
		Queue<BattleAction> imminentActions = new Queue<BattleAction>();
		BattleRunner runner;

		public BattleTimeline(BattleRunner runner)
        {
			this.runner = runner;
        }

		public void EnqueueAction(Func<IEnumerator> func, int timeUntil)
        {
			var ba = new BattleAction
			{
				func = func,
				time = Time + timeUntil,
			};
			if (timeUntil <= 0)
            {
				imminentActions.Enqueue(ba);
				if (!playing)
					runner.StartCoroutine(PlayImminentActions());
			} else
            {
				futureActions.Enqueue(ba);
			}
        }

		public void Tick()
        {
			Time++;
			while (futureActions.TryDequeue(out var next) && Time >= next.time)
            {
				imminentActions.Enqueue(next);
				if (!playing)
					runner.StartCoroutine(PlayImminentActions());
            }
        }

		bool playing;
		IEnumerator PlayImminentActions()
        {
			playing = true;
			var imm = imminentActions;
			while (imm.TryDequeue(out var crnt))
            {
				yield return runner.StartCoroutine(crnt.func.Invoke());
            }
			playing = false;
        }

		public class BattleAction
        {
			public int time;
			public Func<IEnumerator> func;
        }
	}
}
