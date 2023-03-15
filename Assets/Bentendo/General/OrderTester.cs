using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class OrderTester : MonoBehaviour
	{
        public bool greatestFirst;
        public bool firstComeFirstServed;
        public void Start()
        {
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                RerunExperiment();
        }

        private void RerunExperiment()
        {
            var queue = new OrderedQueue<Person>(value => value.Age, greatestFirst, firstComeFirstServed);
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
                Debug.Log(queue.Dequeue().ToString() + " then ");
            Debug.Log("end");
        }

        class Person
        {
			public string Name;
			public int Age;
			public float Height;

            public override string ToString()
            {
                return Name + ", " + Age + "\n";
            }
        }
	}
}
