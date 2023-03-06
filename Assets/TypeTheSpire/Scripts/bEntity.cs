using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class bEntity
{
    public float health { get; private set; }
    public float block { get; private set; }
    public UnityEvent UpdateUI = new UnityEvent();
    UnityAction<bEntity> onDeathAction;

    public float maxHealth { get; private set; }

    public bEntity(float maxHP, UnityAction<bEntity> onDeath)
    {
        this.maxHealth = maxHP;
        this.health = maxHP;
        this.onDeathAction = onDeath;
    }

    public void DealDamage(float value)
    {
        if (value > block)
        {
            value -= block;
            block = 0f;
            health -= value;
        } else
        {
            block -= value;
        }
        if (health <= 0f)
        {
            onDeathAction.Invoke(this);
        }
        UpdateUI.Invoke();
    }

    public void Heal(float value)
    {
        health = Mathf.Min(maxHealth, health + value);
        UpdateUI.Invoke();
    }

    public void AddBlock(float value)
    {
        block += value;
        UpdateUI.Invoke();
    }

    public void EndTurn()
    {
        block = 0f;
        UpdateUI.Invoke();
    }

    public void SetDeathAction(UnityAction<bEntity> action)
    {
        this.onDeathAction = action;
    }
}
