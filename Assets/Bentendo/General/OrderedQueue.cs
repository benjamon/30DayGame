using System;

namespace Bentendo.TTS
{
    public class OrderedQueue<Item>
    {
		Node next = null;
		Func<Item, float> GetItemValue;
		bool greatestFirst = false;

		public OrderedQueue(Func<Item, float> func, bool greatestFirst = false)
        {
			GetItemValue = func;
			this.greatestFirst = greatestFirst;
        }

		public Item Peek() => next.item;

		public Item Dequeue()
        {
			var node = next;
			next = node.prev;
			node.Dequeue();
			return node.item;
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
				while (crnt.prev != null && crnt.prev != node)
				{
					if ((GetItemValue(crnt.item) < nodeValue) == greatestFirst)
                    {
						node.Insert(crnt, pcrnt);
						return;
                    }
					pcrnt = crnt;
					crnt = crnt.prev;
				}
				node.Insert(crnt, pcrnt);
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
