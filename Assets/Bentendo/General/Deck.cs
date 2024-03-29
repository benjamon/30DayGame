using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Deck<T>
{
    public Queue<T> DrawPile;
    public List<T> Discard = new List<T>();

    public Deck(List<T> cards)
    {
        DrawPile = new Queue<T>(cards);
        Shuffle();
    }
    public Deck(T[] cards)
    {
        DrawPile = new Queue<T>(cards);
        Shuffle();
    }

    public void Shuffle()
    {
        while (DrawPile.Count > 0)
            Discard.Add(DrawPile.Dequeue());
        while (Discard.Count > 0)
        {
            int n = UnityEngine.Random.Range(0, Discard.Count);
            DrawPile.Enqueue(Discard[n]);
            Discard.RemoveAt(n);
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
