using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityActor : MonoBehaviour
{
    public TMP_Text health;
    public TMP_Text armor;
    public SpriteRenderer intentImage;
    public TMP_Text intentValue;
    public bEntity entity { get; private set; }

    public Sprite[] intents;

    public void Setup(bEntity entity)
    {
        this.entity = entity;
        entity.UpdateUI.AddListener(UpdateUI);
        UpdateUI();
    }

    private void UpdateUI()
    {
        health.text = Mathf.FloorToInt(entity.health) + "/" + Mathf.FloorToInt(entity.maxHealth);
        armor.text = Mathf.FloorToInt(entity.block).ToString();
    }

    internal void ShowIntent(WordSpellAction wordSpellAction)
    {
        intentValue.text = Mathf.FloorToInt(wordSpellAction.amount).ToString();
        switch (wordSpellAction.actionType)
        {
            case wSpellAction.block:
                intentImage.sprite = intents[0];
                break;
            case wSpellAction.attack:
            default:
                intentImage.sprite = intents[1];
                break;
        }
    }
}
