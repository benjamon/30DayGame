using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityActor : MonoBehaviour
{
    public TMP_Text health;
    public TMP_Text armor;
    public SpriteRenderer charImage;
    public SpriteRenderer intentImage;
    public TMP_Text intentValue;
    public StatusDisplay statuses;
    public bEntity entity { get; private set; }

    public virtual void Setup(bEntity entity)
    {
        this.entity = entity;
        entity.UpdateUI.AddListener(UpdateUI);
        statuses.Setup(entity);
        UpdateUI();
    }

    private void UpdateUI()
    {
        health.text = Mathf.FloorToInt(entity.health) + "/" + Mathf.FloorToInt(entity.maxHealth);
        armor.text = Mathf.FloorToInt(entity.block).ToString();
    }

    internal void ShowIntent(WordCard card)
    {
        intentValue.text = Mathf.FloorToInt(card.spell.action.amount).ToString();
        intentImage.sprite = card.spell.icon;
    }

    Deck<WordCard> actionDeck;
    WordCard lastAction;
    public WordCard GetNext()
    {
        if (lastAction != null)
        {
            actionDeck.AddToDiscard(lastAction);
        }
        if (actionDeck.DrawPileEmpty())
            actionDeck.Shuffle();
        if (!actionDeck.TryDrawNext(out WordCard card))
            Debug.LogError(gameObject.name + " has no cards");
        lastAction = card;
        return card;
    }

    internal void SetDeck(Deck<WordCard> deck)
    {
        actionDeck = deck;
    }
}
public enum eSpellCastAnimations
{
    b,
    smallAttack,
    defend,
    debuff,
}
