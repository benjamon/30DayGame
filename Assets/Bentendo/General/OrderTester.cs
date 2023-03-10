using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class OrderTester : MonoBehaviour
	{
        public void Start()
        {
            var queue = new OrderedQueue<Person>(value => value.Age);
            queue.Enqueue(new Person
            {
                Name = "Ben",
                Age = 28,
                Height = 6.2f,
            });
            queue.Enqueue(new Person
            {
                Name = "Zane",
                Age = 30,
                Height = 5.11f,
            });
            queue.Enqueue(new Person
            {
                Name = "Alfred",
                Age = 22,
                Height = 4.11f,
            });
            queue.Enqueue(new Person
            {
                Name = "Greko",
                Age = 28,
                Height = 6.5f,
            });
            while (queue.Peek() != null)
                Debug.Log(queue.Dequeue().Name);
        }
        class Person
        {
			public string Name;
			public int Age;
			public float Height;
        }
	}
}
