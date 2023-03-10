using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypeTheSpire : MonoBehaviour
{
    public static TypeTheSpire Instance;

    public WordTyper kInput;
    public HandDisplay handManager;
    WordProvider words;

    public WordSpell[] starterDeck;
    public int handSize = 5;
    Deck<WordCard> deck;
    public Dictionary<char, TargetWord> active = new Dictionary<char, TargetWord>();

    private void Awake()
    {
        Instance = this;
        if (WordProvider.Instance == null)
        {
            Debug.LogError("word provider not found");
            SceneManager.LoadScene(0);
            return;
        }
        var cards = new List<WordCard>();
        foreach (var spell in starterDeck)
            cards.Add(new WordCard(spell));
        deck = new Deck<WordCard>(cards.ToArray());
        words = WordProvider.Instance;
    }

    public void TryFind(char c)
    {
        if (active.ContainsKey(c))
        {
            active[c].ProcessCharacter(c);
            kInput.SetTarget(active[c].ProcessCharacter);
        }
    }

    internal void DrawCards()
    {
        active.Clear();
        kInput.SetTarget(TryFind);
        WordCard[] cards = new WordCard[handSize];
        for (int i = 0; i < handSize; i++)
        {
            if (deck.DrawPile.Count == 0)
                deck.Shuffle();
            if (!deck.TryDrawNext(out WordCard card))
                Debug.LogError("no card to draw");
            cards[i] = card;
        }
        handManager.ShowHand(cards);
    }

    internal void ShowHand()
    {
        handManager.ShowWords();
    }

    public AudioSource successSound;
    public void PlayCard(WordCard card)
    {
        card.spell.action.InvokeOn(hero, enemy);
        successSound.Play();
        kInput.SetTarget(TryFind);
        deck.AddToDiscard(card);
    }

    internal void DiscardHand()
    {
        active.Clear();
        handManager.ClearAll();
    }

    internal void Discard(WordCard card)
    {
        deck.AddToDiscard(card);
    }

    bEntity enemy;
    bEntity hero;

    public void DefineEntities(bEntity hero, bEntity enemy)
    {
        this.hero = hero;
        this.enemy = enemy;
    }

    public void WordCompleted(TargetWord word)
    {
        kInput.SetTarget(TryFind);
    }
}
