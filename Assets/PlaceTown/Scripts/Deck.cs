using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Deck<T>
{
    public Queue<T> DrawPile;
    public List<T> Discard = new List<T>();

    public Deck(T[] cards)
    {
        DrawPile = new Queue<T>(cards);
    }

    public void Shuffle()
    {
        while (DrawPile.Count > 0)
        {
            Discard.Add(DrawPile.Dequeue());
        }
        while (Discard.Count > 0)
        {
            int n = UnityEngine.Random.Range(0, Discard.Count - 1);
            DrawPile.Enqueue(Discard[n]);
            Discard.RemoveAt(n);
        }
    }

    public IEnumerator ShuffleRoutine(Action afterSwap, float time)
    {
        while (DrawPile.Count > 0)
        {
            Discard.Add(DrawPile.Dequeue());
        }
        while (Discard.Count > 0)
        {
            int n = UnityEngine.Random.Range(0, Discard.Count - 1);
            DrawPile.Enqueue(Discard[n]);
            Discard.RemoveAt(n);
            afterSwap.Invoke();
            yield return new WaitForSeconds(time);
        }
    }

    public bool DrawPileEmpty() => DrawPile.Count == 0;
    public void AddToDiscard(T item) => Discard.Add(item);

    public bool TryDrawNext(out T next) => DrawPile.TryDequeue(out next);

    public bool TryPeekNext(out T next) => DrawPile.TryPeek(out next);

    public bool TryGetLastDiscarded(out T last) {
        if (Discard.Count > 0)
        {
            last = Discard[Discard.Count - 1];
            return true;
        }
        last = default;
        return false;
    }
}
