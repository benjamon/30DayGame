using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Bentendo.TTS
{
	public class BattleTimeline
	{
		public float CurrentTime { get; private set; }
		public int CurrentTick { get; private set; }
		public float TimeScale = 1f;
		public float TicksPerSecond;

		public BattleActionEvent OnActionAdded = new BattleActionEvent();
		public BattleActionEvent OnActionImminent = new BattleActionEvent();

		OrderedQueue<BattleAction> futureActions = new OrderedQueue<BattleAction>(val => val.tick);
		Queue<BattleAction> imminentActions = new Queue<BattleAction>();
		BattleRunner runner;


		public BattleTimeline(BattleRunner runner, float ticksPerSecond)
        {
			this.runner = runner;
			TicksPerSecond = ticksPerSecond;
        }

		public BattleAction EnqueueAction(Func<IEnumerator> func, int ticksUntil)
        {
			var ba = new BattleAction(CurrentTick + ticksUntil, func);
			if (ticksUntil <= 0)
            {
				OnActionImminent.Invoke(ba);
				imminentActions.Enqueue(ba);
				ba.OnImminent.Invoke();
				if (!playing)
					runner.StartCoroutine(PlayImminentActions());
			} else
            {
				futureActions.Enqueue(ba);
				OnActionAdded.Invoke(ba);
			}
			return ba;
        }

		public void AddTime(float delta)
        {
			CurrentTime += delta * TimeScale * TicksPerSecond;
			int targetTick = UnityEngine.Mathf.FloorToInt(CurrentTime);
			while (CurrentTick < targetTick)
				ProcessTick();
        }

		void ProcessTick()
        {
			CurrentTick++;
			var crnt = futureActions.Peek();
			while (crnt != null && CurrentTick >= crnt.tick)
            {
				imminentActions.Enqueue(futureActions.Dequeue());
				OnActionImminent.Invoke(crnt);
				crnt.OnImminent.Invoke();
				crnt = futureActions.Peek();
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
				crnt.OnCastStart.Invoke();
				yield return runner.StartCoroutine(crnt.func.Invoke());
				crnt.OnCastComplete.Invoke();
            }
			playing = false;
        }

		public class BattleActionEvent : UnityEvent<BattleAction> { }
    }

    public class BattleAction
    {
        int _tick;
        public int tick
        {
            get => _tick;
            set
            {
                _tick = value;
                OnTimeChanged.Invoke(_tick);
            }
        }

        public Func<IEnumerator> func;
        public UnityEvent OnImminent = new UnityEvent();
        public UnityEvent OnRemoved = new UnityEvent();
        public UnityEvent OnCastStart = new UnityEvent();
        public UnityEvent OnCastComplete = new UnityEvent();
        public TimeChangeEvent OnTimeChanged = new TimeChangeEvent();

        public BattleAction(int time, Func<IEnumerator> func)
        {
            this._tick = time;
            this.func = func;
        }

        public class TimeChangeEvent : UnityEvent<int> { }
    }
}
