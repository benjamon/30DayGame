using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Bentendo.TTS.BattleTimeline;

namespace Bentendo.TTS
{
	public class TimelineUI : MonoBehaviour
	{
		public GameObject EventPrefab;
		public GameObject ImminentPrefab;
		public Transform TimelineParent;
		public Transform ImminentParent;

		public Vector3 ImminentQueueShift;
		public Vector3 tickDist;

		List<EventNode> futureNodes = new List<EventNode>();

		List<EventNode> imminentNodes = new List<EventNode>();

		BattleTimeline timeline;
		int lastPlaced;
		bool isSetup;

		public void Setup(BattleTimeline timeline)
        {
			this.timeline = timeline;
			//tickScale = calculated distance / future_ticks_to_show
			timeline.OnActionAdded.AddListener(AddNodeToTimeline);
			timeline.OnActionImminent.AddListener(AddNodeToImminentQueue);
			isSetup = true;
        }

        private void AddNodeToImminentQueue(BattleAction action)
		{
			var go = GameObject.Instantiate(ImminentPrefab, ImminentParent);
			var en = new EventNode(go, action, imminentNodes, ImminentQueueShift);
			action.OnCastStart.AddListener(() => {
				en.KillNode();
				ImminentParent.position -= ImminentQueueShift;
			});
			en.PlaceNode(lastPlaced++);
		}

        public void AddNodeToTimeline(BattleAction action) 
		{
			var go = GameObject.Instantiate(EventPrefab, TimelineParent);
			var en = new EventNode(go, action, futureNodes, tickDist);
			action.OnImminent.AddListener(en.KillNode);
		}

        private void Update()
        {
            if (!isSetup)
                return;
            TimelineParent.transform.localPosition = -timeline.CurrentTime * tickDist;
        }

        class EventNode
        {
			public BattleAction action;
			public GameObject visual;

			Vector3 stepShift;
			List<EventNode> nodeList;

			public EventNode(GameObject visual, BattleAction action, List<EventNode> nodeList, Vector3 stepShift)
            {
				this.action = action;
				this.visual = visual;
				this.stepShift = stepShift;
				this.nodeList = nodeList;
				nodeList.Add(this);
				PlaceNode(action.executeOnTick);
				action.OnTimeChanged.AddListener(PlaceNode);
				action.OnCancelled.AddListener(CancelAction);
			}

			public void KillNode()
            {
				//return to pool lol
				GameObject.Destroy(visual);
				nodeList.Remove(this);
            }

			public void PlaceNode(int tick)
			{
				visual.transform.localPosition = tick * stepShift;
			}

			void CancelAction()
			{
				switch (action.Status)
				{
					case BattleActionStatus.Future:
					case BattleActionStatus.Imminent:
						Debug.Log("canceling " + action.Status.ToString() + " action");
						KillNode();
						break;
					case BattleActionStatus.Casting:
						throw new Exception("cancelling during cast not yet implemented");
					case BattleActionStatus.Completed:
						throw new Exception("tried canceling node when status completed");
					case BattleActionStatus.Cancelled:
						throw new Exception("tried canceling node when status completed");
				}
			}
        }
    }
}
