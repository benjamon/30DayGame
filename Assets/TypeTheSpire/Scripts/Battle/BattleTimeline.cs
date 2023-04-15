using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

		OrderedQueue<BattleAction> futureActions = new OrderedQueue<BattleAction>(val => val.executeOnTick);
		Queue<BattleAction> imminentActions = new Queue<BattleAction>();
		BattleRunner runner;


		public BattleTimeline(BattleRunner runner, float ticksPerSecond)
        {
			this.runner = runner;
			TicksPerSecond = ticksPerSecond;
        }

		public BattleAction EnqueueAction(Entity source, Func<IEnumerator> func, Sprite icon, int ticksUntil)
        {
			var ba = new BattleAction(source, CurrentTick + ticksUntil, func, icon);
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
				ba.OnCancelled.AddListener(() => RemoveAction(ba));
				OnActionAdded.Invoke(ba);
			}
			return ba;
        }

		void RemoveAction(BattleAction ba)
		{
			if (ba.Status == BattleActionStatus.Future)
				futureActions.TryRemoveItem(ba);
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
			while (crnt != null && CurrentTick >= crnt.executeOnTick)
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
			while (imm.TryPeek(out var crnt))
			{
				yield return runner.StartCoroutine(crnt.InvokeActionRoutine());
				imm.Dequeue();
            }
			playing = false;
        }

		public class BattleActionEvent : UnityEvent<BattleAction> { }
    }

    public class BattleAction
    {
        int _tick;
        public int executeOnTick
        {
            get => _tick;
            set
            {
                _tick = value;
                OnTimeChanged.Invoke(_tick);
            }
		}
		public BattleActionStatus Status
		{
			get; private set;
		}

		public UnityEvent OnImminent = new UnityEvent();
        public UnityEvent OnCancelled = new UnityEvent();
        public UnityEvent OnCastStart = new UnityEvent();
        public UnityEvent OnCastComplete = new UnityEvent();
        public TimeChangeEvent OnTimeChanged = new TimeChangeEvent();

		public Sprite icon;

		Entity source;
		Func<IEnumerator> actionRoutine;

		public BattleAction(Entity source, int time, Func<IEnumerator> func, Sprite icon)
        {
            this._tick = time;
            this.actionRoutine = func;
			this.source = source;
			this.Status = BattleActionStatus.Future;
			this.icon = icon;
			source.OnDeceased.AddListener(CancelAction);
		}

		public IEnumerator InvokeActionRoutine()
        {
			if (Status == BattleActionStatus.Cancelled)
				yield break;
			Status = BattleActionStatus.Casting;
			OnCastStart.Invoke();
			yield return actionRoutine.Invoke();
			OnCastComplete.Invoke();
			Status = BattleActionStatus.Completed;
			source.OnDeceased.RemoveListener(CancelAction);
		}

		public void SetImminent()
		{
			Status = BattleActionStatus.Imminent;
			OnImminent.Invoke();
		}

		public void CancelAction()
		{
			if (Status >= BattleActionStatus.Casting)
				return;
			source.OnDeceased.RemoveListener(CancelAction);
			OnCancelled.Invoke();
			Status = BattleActionStatus.Cancelled;
		}

        public class TimeChangeEvent : UnityEvent<int> { }
    }

	public enum BattleActionStatus
    {
		Future,
		Imminent,
		Casting,
		Completed,
		Cancelled,
    }
}
