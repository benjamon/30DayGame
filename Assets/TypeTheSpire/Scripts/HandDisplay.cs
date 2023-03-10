using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDisplay : MonoBehaviour
{
    public Transform[] CardContainers;
    public GameObject CardPrefab;

    List<WordCardManifestation> activeCards = new List<WordCardManifestation>();

    public void ShowHand(WordCard[] acards)
    {
        var cards = new List<WordCard>(acards);

        for (int i = 0; i < cards.Count; i++)
        {
            var go = GameObject.Instantiate(CardPrefab, CardContainers[i]);
            var crdm = go.GetComponent<WordCardManifestation>();
            activeCards.Add(crdm);
            crdm.Setup(cards[i]);
        }
    }

    public void ShowWords()
    {
        foreach (var card in activeCards)
        {
            card.ShowWord();
        }
    }

    internal void ClearAll()
    {
        foreach (var card in activeCards)
        {
            TypeTheSpire.Instance.Discard(card.card);
            Destroy(card.gameObject);
        }

        activeCards.Clear();
    }
}
