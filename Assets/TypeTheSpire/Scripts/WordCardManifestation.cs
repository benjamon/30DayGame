using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCardManifestation : MonoBehaviour
{
    public SpriteRenderer icon;
    public TargetWord word;
    [HideInInspector]
    public WordCard card;

    public void Setup(WordCard card)
    {
        this.card = card;
        var dict = TypeTheSpire.Instance.active;
        var wrd = WordProvider.Instance.GetRandom(card.spell.wordLength);
        while (dict.ContainsKey(wrd[0])) 
            wrd = WordProvider.Instance.GetRandom(card.spell.wordLength);
        word.Init(wrd);
        TypeTheSpire.Instance.active.Add(word.word[0], word);
        icon.sprite = card.spell.icon;
        word.OnComplete.AddListener(PlayThis);
    }

    private void PlayThis()
    {
        TypeTheSpire.Instance.PlayCard(card);
        gameObject.SetActive(false);
    }
}
