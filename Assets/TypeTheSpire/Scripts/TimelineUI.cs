using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Bentendo.TTS.BattleTimeline;

namespace Bentendo.TTS
{
	public class TimelineUI : MonoBehaviour
	{
		public GameObject EventPrefab;
		public Transform NodeParent;
		public float tickScale { get; private set; }

		List<EventNode> nodes = new List<EventNode>();

		BattleTimeline timeline;

		public void Setup(BattleTimeline timeline)
        {
			this.timeline = timeline;
			//tickScale = calculated distance / future_ticks_to_show
			tickScale = .1f;
			timeline.ActionAdded.AddListener(AddNode);
		}

		public void AddNode(BattleAction action) 
		{
			var go = GameObject.Instantiate(EventPrefab, NodeParent);
			var en = new EventNode(go, action, this);
			nodes.Add(en);
		}

        private void Update()
        {
			NodeParent.transform.localPosition = new Vector3(-timeline.CurrentTime * tickScale, 0f, 0f);
        }

        class EventNode
        {
			public BattleAction action;
			public GameObject visual;

			private TimelineUI timelineUI;

			public EventNode(GameObject visual, BattleAction action, TimelineUI timeline)
            {
				this.action = action;
				this.visual = visual;
				this.timelineUI = timeline;
				PlaceNode(action.tick);
				action.OnImminent.AddListener(() => visual.SetActive(false));
				action.OnTimeChanged.AddListener(PlaceNode);
            }

			public void PlaceNode(int tick)
			{
				visual.transform.localPosition = new Vector3(tick * timelineUI.tickScale, 0f, 0f);
			}
        }
	}
}
