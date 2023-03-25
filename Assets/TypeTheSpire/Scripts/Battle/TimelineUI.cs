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

		List<EventNode> nodes = new List<EventNode>();

		BattleTimeline timeline;
		Vector3 tickDist;
		int lastPlaced;

		public void Setup(BattleTimeline timeline)
        {
			this.timeline = timeline;
			//tickScale = calculated distance / future_ticks_to_show
			timeline.OnActionAdded.AddListener(AddNodeToTimeline);
			timeline.OnActionImminent.AddListener(AddNodeToImminentQueue);
			tickDist = new Vector3(.1f, 0f, 0f);
		}

        private void AddNodeToImminentQueue(BattleAction action)
		{
			var go = GameObject.Instantiate(ImminentPrefab, ImminentParent);
			var en = new EventNode(go, action, ImminentQueueShift);
			action.OnCastStart.AddListener(() => {
				en.KillNode();
				ImminentParent.position -= ImminentQueueShift;
			});
			en.PlaceNode(lastPlaced++);
			nodes.Add(en);
		}

        public void AddNodeToTimeline(BattleAction action) 
		{
			var go = GameObject.Instantiate(EventPrefab, TimelineParent);
			var en = new EventNode(go, action, tickDist);
			action.OnImminent.AddListener(en.KillNode);
			nodes.Add(en);
		}

        private void Update()
        {
			TimelineParent.transform.localPosition = -timeline.CurrentTime * tickDist;
        }

        class EventNode
        {
			public BattleAction action;
			public GameObject visual;

			Vector3 stepShift;

			public EventNode(GameObject visual, BattleAction action, Vector3 stepShift)
            {
				this.action = action;
				this.visual = visual;
				this.stepShift = stepShift;
				PlaceNode(action.tick);
				action.OnTimeChanged.AddListener(PlaceNode);
            }

			public void KillNode()
            {
				//return to pool lol
				GameObject.Destroy(visual);
            }

			public void PlaceNode(int tick)
			{
				visual.transform.localPosition = tick * stepShift;
			}
        }
	}
}
