using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    const int MS_PER_CORO_CHECKUP = 12;

    public static Sequencer main;
    static Sequencer[] all = new Sequencer[26];

    [SerializeField]
    private bool is_main;

    CancellationTokenSource cts;
    Task last;

    public Sequencer this[char c] => all[(int)c - 'a' + 1];
    public Sequencer this[string s] => all[(int)(s.ToLower()[0]) - 'a' + 1];

    public static Sequencer get(char c) => all[(int)c - 'a' + 1];
    public static Sequencer get(string s) => all[(int)(s.ToLower()[0]) - 'a' + 1];

    private List<Action> actionQueue = new();
    private List<Action> actionQueueCopy = new();
    private volatile bool queueEmpty;

    private void Awake()
    {
        if (is_main)
        {
            main = this;

            for (int i = 0; i < all.Length; i++) all[i] = null;
            all[0] = this;
            var allSequencers = GameObject.FindObjectsOfType<Sequencer>();
            foreach (var sequencer in allSequencers)
            {
                if (sequencer == this)
                    continue;
                int i = sequencer.gameObject.name.ToLower()[0] - 'a' + 1;
                all[i] = sequencer;
            }
        }
    }

    private void Update()
    {
        if (queueEmpty)
            return;

        lock(actionQueue)
        {
            actionQueueCopy.AddRange(actionQueue);
            actionQueue.Clear();
            queueEmpty = true;
        }

        for (int i = 0; i < actionQueueCopy.Count; i++)
            actionQueueCopy[i].Invoke();

        actionQueueCopy.Clear();
    }

    private void OnDestroy()
    {
        cts?.Cancel();
    }

    public void EscapeSequence()
    {
        cts?.Cancel();
    }

    public void Enqueue(Func<Task> ft)
    {
        if (last == null || last.IsCompleted)
        {
            cts = new();
            last = Task.Run(() => ft(), cts.Token);
        } else
        {
            last = last.Then(ft, cts.Token);
        }
    }

    #region Enqueue Overloads
    public void Enqueue<T>(Func<T, Task> ft, T v1)
    {
        if (last == null ||  last.IsCompleted)
        {
            cts = new();
            last = Task.Run(() => ft(v1), cts.Token);
        }
        else
        {
            last = last.Then(ft, v1, cts.Token);
        }
    }

    public void Enqueue<T, G>(Func<T, G, Task> ft, T v1, G v2)
    {
        if (last == null || last.IsCompleted)
        {
            cts = new();
            last = Task.Run(() => ft.Invoke(v1, v2), cts.Token);
        }
        else
        {
            last = last.Then(ft, v1, v2, cts.Token);
        }
    }

    public void  Enqueue<T, G, K>(Func<T, G, K, Task> ft, T v1, G v2, K v3)
    {
        if (last == null || last.IsCompleted)
        {
            cts = new();
            last = Task.Run(() => ft.Invoke(v1, v2, v3), cts.Token);
        }
        else
        {
            last = last.Then(ft, v1, v2, v3, cts.Token);
        }
    }

    public void Enqueue(Func<IEnumerator> c)
    {
        Enqueue(RunTaskRoutine, c);
    }

    public void Enqueue<T>(Func<T, IEnumerator> c, T v1)
    {
        Enqueue(RunTaskRoutine, c, v1);
    }

    public void Enqueue<T, G>(Func<T, G, IEnumerator> c, T v1, G v2)
    {
        Enqueue(RunTaskRoutine, c, v1, v2);
    }

    public void Enqueue(Action a)
    {
        if (last == null || last.IsCompleted)
        {
            cts = new();
            last = Task.Run(() => PlayAction(a), cts.Token);
        }
        else
        {
            last = last.Then(PlayAction, a, cts.Token);
        }
    }

    public void WaitUntil(Func<bool> condition)
    {
        Enqueue(async () =>
        {
            var c = condition;
            while (!c())
                await Task.Delay(MS_PER_CORO_CHECKUP);
        });
    }
    #endregion

    //https://stackoverflow.com/questions/58469468/what-does-unitymainthreaddispatcher-do I broke coroutine await :O
    #region Coroutine Lock Helpers

    public async Task RunTaskRoutine(System.Func<IEnumerator> c)
    {
        RoutineLock rlock = new();
        System.Action action = () =>
        {
            StartCoroutine(WrapRoutine(StartCoroutine(c()), rlock));
        };
        lock (actionQueue)
        {
            actionQueue.Add(action);
            queueEmpty = false;
        }
        while (!rlock.IsComplete)
        {
            await Task.Delay(MS_PER_CORO_CHECKUP);
        }
    }

    public async Task RunTaskRoutine<T>(System.Func<T, IEnumerator> c, T v1)
    {
        RoutineLock rlock = new();
        System.Action action = () =>
        {
            StartCoroutine(WrapRoutine(StartCoroutine(c(v1)), rlock));
        };
        lock (actionQueue)
        {
            actionQueue.Add(action);
            queueEmpty = false;
        }
        while (!rlock.IsComplete)
        {
            await Task.Delay(MS_PER_CORO_CHECKUP);
        }
    }

    public async Task RunTaskRoutine<T, G>(System.Func<T, G, IEnumerator> c, T v1, G v2)
    {
        RoutineLock rlock = new();
        System.Action action = () =>
        {
            StartCoroutine(WrapRoutine(StartCoroutine(c(v1, v2)), rlock));
        };
        lock (actionQueue)
        {
            actionQueue.Add(action);
            queueEmpty = false;
        }
        while (!rlock.IsComplete)
        {
            await Task.Delay(MS_PER_CORO_CHECKUP);
        }
    }

    IEnumerator WrapRoutine(Coroutine c, RoutineLock l)
    {
        yield return c;
        l.IsComplete = true;
    }

    class RoutineLock
    {
        public bool IsComplete;
    }

    #endregion

    #region Actions

    public async Task PlayAction(Action a)
    {
        a.Invoke();
        await Task.Delay(1);
    }
    #endregion

    #region examples or unit tests or whatever
    void QueueBreakfast()
    {
        Enqueue(LogTask, 111);
        Enqueue(LogTask, 222);
        Enqueue(LogTask, 333);
        Func<Task> grupo = () =>
        {
            Task t1 = LogTask(4);
            Task t2 = LogTask(5);
            Task t3 = LogTask(6);
            return Task.WhenAll(t1, t2, t3);
        };
        Enqueue(grupo);
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
    #endregion
}
public static class TaskExtensions
{
    public static async Task Then(this Task task, Func<Task> continuation, CancellationToken ct)
    {
        await task;
        if (ct.IsCancellationRequested)
        {
            Debug.Log("canceleed");
            return;
        }
        await continuation();
    }

    public static async Task Then<T>(this Task task, Func<T, Task> continuation, T v1, CancellationToken ct)
    {
        await task;
        if (ct.IsCancellationRequested)
        {
            Debug.Log("canceleed");
            return;
        }
        await continuation(v1);
    }

    public static async Task Then<T, G>(this Task task, Func<T, G, Task> continuation, T v1, G v2, CancellationToken ct)
    {
        await task;
        if (ct.IsCancellationRequested)
        {
            Debug.Log("canceleed");
            return;
        }
        await continuation(v1, v2);
    }
    public static async Task Then<T, G, K>(this Task task, Func<T, G, K, Task> continuation, T v1, G v2, K v3, CancellationToken ct)
    {
        await task;
        if (ct.IsCancellationRequested)
        {
            Debug.Log("canceleed");
            return;
        }
        await continuation(v1, v2, v3);
    }
}