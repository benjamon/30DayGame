using System.Collections;
using System.Collections.Generic;
using Bentendo;
using NUnit.Framework;
using UnityEngine;
using tt = UnityEngine.TestTools;

public class OrderedQueueTests
{
    [Test(Description = "Person objects come out of OrderedQueue in descending order, when greatestFirstFalse")]
    public void OrderedQueueGreatestFirstPasses()
    {
        var queue = new OrderedQueue<TestPerson>(val => val.age, true, false);
        EnqueueTestPeople(queue);
        var biggest = new TestPerson
        {
            name = "bigs",
            age = 100,
        };
        queue.Enqueue(biggest);
        var next = queue.Peek();
        int lastAge = next.age;
        Assert.AreEqual(next, biggest);
        while (next != null)
        {
            Assert.LessOrEqual(next.age, lastAge);
            lastAge = next.age;
            queue.Dequeue();
            next = queue.Peek();
        }
    }

    [Test(Description = "Person objects come out of OrderedQueue in ascending order, when greatestFirstFalse")]
    public void OrderedQueueSmallestFirstPasses()
    {
        // Use the Assert class to test conditions
        var queue = new OrderedQueue<TestPerson>(val => val.age, false, false);
        EnqueueTestPeople(queue);
        var smalls = new TestPerson
        {
            name = "bigs",
            age = 1,
        };
        queue.Enqueue(smalls);
        var next = queue.Peek();
        int lastAge = next.age;
        Assert.AreEqual(next, smalls);
        while (next != null)
        {
            Assert.GreaterOrEqual(next.age, lastAge);
            lastAge = next.age;
            queue.Dequeue();
            next = queue.Peek();
        }
    }

    [Test(Description = "Ties decided by order enqueued and the firstFirstServed parameter")]
    public void OrderedQueueFirstComeFirstServedWorks()
    {
        var queue = new OrderedQueue<TestPerson>(val => val.age, false, true);
        queue.Enqueue(new TestPerson
        {
            name = "joe",
            age = 1,
        });
        queue.Enqueue(new TestPerson
        {
            name = "bob",
            age = 1,
        });
        queue.Enqueue(new TestPerson
        {
            name = "rob",
            age = 1,
        });

        Assert.AreEqual(queue.Peek().name, "joe");

        queue = new OrderedQueue<TestPerson>(val => val.age, false, false);
        queue.Enqueue(new TestPerson
        {
            name = "joe",
            age = 1,
        });
        queue.Enqueue(new TestPerson
        {
            name = "bob",
            age = 1,
        });
        queue.Enqueue(new TestPerson
        {
            name = "rob",
            age = 1,
        });
        Assert.AreEqual(queue.Peek().name, "rob");
    }

    [Test(Description = "TryDequeue is false, dequeue is null when empty")]
    public void OrderedQueueReturnsNullWhenEmpty()
    {
        var queue = new OrderedQueue<TestPerson>(val => val.age);
        Assert.AreEqual(queue.Dequeue(), null);
        Assert.False(queue.TryDequeue(out TestPerson person));
    }

    [Test(Description = "Foreach iterates over all items in the queue")]
    public void OrderedQueueForeachIteratesOverAllItems()
    {
        var queue = new OrderedQueue<TestPerson>(val => val.age);
        EnqueueTestPeople(queue);
        int n = 0;
        foreach (var tp in queue)
            n++;
        Assert.AreEqual(n, 4);
    }

    void EnqueueTestPeople(OrderedQueue<TestPerson> queue)
    {
        queue.Enqueue(new TestPerson
        {
            name = "joe",
            age = 10,
        });
        queue.Enqueue(new TestPerson
        {
            name = "bob",
            age = 12,
        });
        queue.Enqueue(new TestPerson
        {
            name = "dil",
            age = 9,
        });
        queue.Enqueue(new TestPerson
        {
            name = "ben",
            age = 11,
        });
    }

    class TestPerson 
    {
        public string name;
        public int age;
    }
}
