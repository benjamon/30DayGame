using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    const int MS_PER_CORO_CHECKUP = 12;

    public static Sequencer MainThread;

    Task last;

    private void Awake()
    {
        MainThread = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MakeBreakfast();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            QueueBreakfast();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlanBreakfastRoutine();
        }
    }

    void EnqueueTask(Func<Task> ft)
    {
        if (last == null || last.IsCompleted)
        {
            last = ft.Invoke();
        } else
        {
            last = last.Then(ft);
        }
    }

    void EnqueueTask<T>(Func<T, Task> ft, T v1)
    {
        if (last == null || last.IsCompleted)
        {
            last = ft.Invoke(v1);
        }
        else
        {
            last = last.Then(ft, v1);
        }
    }

    void EnqueueTask<T, G>(Func<T, G, Task> ft, T v1, G v2)
    {
        if (last == null || last.IsCompleted)
        {
            last = ft.Invoke(v1, v2);
        }
        else
        {
            last = last.Then(ft, v1, v2);
        }
    }
    void EnqueueTask<T, G, K>(Func<T, G, K, Task> ft, T v1, G v2, K v3)
    {
        if (last == null || last.IsCompleted)
        {
            last = ft.Invoke(v1, v2, v3);
        }
        else
        {
            last = last.Then(ft, v1, v2, v3);
        }
    }

    public void Enqueue(Func<IEnumerator> c)
    {
        EnqueueTask(RunTaskRoutine, c);
    }

    public void Enqueue<T>(Func<T, IEnumerator> c, T v1)
    {
        EnqueueTask(RunTaskRoutine, c, v1);
    }

    public void Enqueue<T, G>(Func<T, G, IEnumerator> c, T v1, G v2)
    {
        EnqueueTask(RunTaskRoutine, c, v1, v2);
    }

    async Task RunTaskRoutine(Func<IEnumerator> c)
    {
        RoutineLock rlock = new RoutineLock();
        StartCoroutine(WrapRoutine(StartCoroutine(c()), rlock));
        while (!rlock.IsComplete)
            await Task.Delay(MS_PER_CORO_CHECKUP);
    }

    async Task RunTaskRoutine<T>(Func<T, IEnumerator> c, T v1)
    {
        RoutineLock rlock = new RoutineLock();
        StartCoroutine(WrapRoutine(StartCoroutine(c(v1)), rlock));
        while (!rlock.IsComplete)
            await Task.Delay(MS_PER_CORO_CHECKUP);
    }

    async Task RunTaskRoutine<T, G>(Func<T, G, IEnumerator> c, T v1, G v2)
    {
        RoutineLock rlock = new RoutineLock();
        StartCoroutine(WrapRoutine(StartCoroutine(c(v1, v2)), rlock));
        while (!rlock.IsComplete)
            await Task.Delay(MS_PER_CORO_CHECKUP);
    }

    IEnumerator WrapRoutine(Coroutine c, RoutineLock l)
    {
        yield return c;
        l.IsComplete = true;
    }

    public class RoutineLock
    {
        public bool IsComplete;
    }

    //examples

    void QueueBreakfast()
    {
        EnqueueTask(LogTask, 111);
        EnqueueTask(LogTask, 222);
        EnqueueTask(LogTask, 333);
        Func<Task> grupo = () =>
        {
            Task t1 = LogTask(4);
            Task t2 = LogTask(5);
            Task t3 = LogTask(6);
            return Task.WhenAll(t1, t2, t3);
        };
        EnqueueTask(grupo);
    }

    void PlanBreakfastRoutine()
    {
        Enqueue(LogRandom);
        Enqueue(LogRandom);
        Enqueue(LogRoutine, "eggs");
        Enqueue(LogRoutine, "bacon");
        Enqueue(LogScore, "best score", 24);
        Enqueue(LogScore, "worst score", 24);
    }

    async void MakeBreakfast()
    {
        await LogTask(1);
        await LogTask(2);

        var t3 = LogTask(3);
        var t4 = LogTask(4);
        var tasks = new Task[] { t3, t4, LogTask(5) };

        await Task.WhenAll(tasks);
    }

    async Task LogTask(int n)
    {
        await Task.Delay(500);
        Debug.Log(n + " complete");
    }

    IEnumerator LogScore(string s, int score)
    {
        yield return new WaitForSeconds(.5f);
        Debug.Log(s + ": " + score);
    }

    IEnumerator LogRoutine(string s)
    {
        yield return new WaitForSeconds(.5f);
        Debug.Log(s);
    }

    IEnumerator LogRandom()
    {
        yield return new WaitForSeconds(.4f);
        Debug.Log(UnityEngine.Random.Range(0, 24));
    }
}
public static class TaskExtensions
{
    public static async Task Then(this Task task, Func<Task> continuation)
    {
        await task;
        await continuation();
    }

    public static async Task Then<T>(this Task task, Func<T, Task> continuation, T v1)
    {
        await task;
        await continuation(v1);
    }

    public static async Task Then<T, G>(this Task task, Func<T, G, Task> continuation, T v1, G v2)
    {
        await task;
        await continuation(v1, v2);
    }
    public static async Task Then<T, G, K>(this Task task, Func<T, G, K, Task> continuation, T v1, G v2, K v3)
    {
        await task;
        await continuation(v1, v2, v3);
    }
}