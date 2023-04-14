using System;
using System.Collections;
using UnityEngine;

namespace Bentendo
{
    public class OrderedQueue<Item> : IEnumerable where Item : class
    {
		Node next = null;
		Func<Item, float> GetItemValue;
		bool greatestFirst = false;
		bool firstComeFirstServed = false;

		public OrderedQueue(Func<Item, float> func, bool greatestFirst = false, bool firstComeFirstServed = true)
        {
			GetItemValue = func;
			this.greatestFirst = greatestFirst;
			this.firstComeFirstServed = firstComeFirstServed;
		}

		public Item Peek() => (next != null) ? next.item : null;

		public bool TryPeek(out Item nextItem)
        {
			if (next != null)
            {
				nextItem = next.item;
				return true;
            }
			nextItem = null;
			return false;
        }
			
		public Item Dequeue()
        {
			if (next == null)
				return null;
			var node = next;
			next = node.prev;
			node.Dequeue();
			return node.item;
		}

		public bool TryDequeue(out Item nextItem)
		{
			var node = next;
			if (node != null)
			{
				nextItem = node.item;
				next = node.prev;
				node.Dequeue();
				return true;
			}
			nextItem = null;
			return false;
		}

		public bool TryRemoveItem(Item item)
		{
			if (next.item == item)
			{
				Dequeue();
				return true;
			}
			var crnt = next.next;
			while (crnt != null)
			{
				if (crnt.item == item)
				{
					crnt.AttachNeighbors();
					return true;
				}
				crnt = crnt.prev;
			}
			return false;
		}

		public void Enqueue(Item item)
        {
			Node node = new Node(item);
			if (next == null)
				next = new Node(item);
			else
			{
				Node pcrnt = null;
				Node crnt = next;
				var nodeValue = GetItemValue(item);
				while (crnt != null && crnt.prev != node)
				{
					var crntValue = GetItemValue(crnt.item);
					if ((crntValue < nodeValue) == greatestFirst && 
						((crntValue != nodeValue) || ((crntValue == nodeValue) && !firstComeFirstServed)))
                    {
						node.Insert(crnt, pcrnt);
						if (pcrnt == null)
							next = node;
						return;
                    }
					pcrnt = crnt;
					crnt = crnt.prev;
				}
				node.Insert(crnt, pcrnt);
			}
        }

        public IEnumerator GetEnumerator()
        {
			var crnt = next;
			while (crnt != null)
            {
				crnt = crnt.prev;
				yield return crnt;
            }
        }

        class Node
		{
			public Node next { get; private set; }
			public Node prev { get; private set; }
			public Item item;

			public Node(Item item)
            {
				this.item = item;
            }

			public void Dequeue()
            {
				if (prev != null)
					prev.next = null;
            }

			public void AttachNeighbors()
            {
				if (prev != null) prev.next = next;
				if (next != null) next.prev = prev;
				next = null;
				prev = null;
			}

			public void Insert(Node prev, Node next)
            {
				this.prev = prev;
				this.next = next;
				if (prev != null)
					prev.next = this;
				if (next != null)
					next.prev = this;
            }
		}
	}
}
