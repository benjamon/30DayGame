using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCardManifestation : MonoBehaviour
{
    public SpriteRenderer icon;
    public TargetWord targetWord;
    [HideInInspector]
    public WordCard card;

    public void Setup(WordCard card)
    {
        this.card = card;
        targetWord.ShowLength(card.spell.wordLength);
        icon.sprite = card.spell.icon;
    }

    public void ShowWord()
    {
        var dict = TypeTheSpire.Instance.active;
        var wrd = WordProvider.Instance.GetRandom(card.spell.wordLength);
        while (dict.ContainsKey(wrd[0]))
            wrd = WordProvider.Instance.GetRandom(card.spell.wordLength);
        targetWord.Init(wrd);
        TypeTheSpire.Instance.active.Add(targetWord.word[0], targetWord);
        targetWord.OnComplete.AddListener(PlayThis);
    }

    private void PlayThis()
    {
        TypeTheSpire.Instance.PlayCard(card);
        gameObject.SetActive(false);
    }
}
