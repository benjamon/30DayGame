using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class bEntity
{
    public int health { get; private set; }
    public int block { get; private set; }
    public UnityEvent UpdateUI = new UnityEvent();
    public UnityEvent StatusUpdate = new UnityEvent();
    public Dictionary<wStatusEffect, StatusData> statusEffects = new Dictionary<wStatusEffect, StatusData>();
    UnityAction<bEntity> onDeathAction;

    public float maxHealth { get; private set; }

    public bEntity(int maxHP, UnityAction<bEntity> onDeath)
    {
        this.maxHealth = maxHP;
        this.health = maxHP;
        this.onDeathAction = onDeath;
    }

    public bool has(wStatusEffect status)
    {
        return statusEffects.ContainsKey(status);
    }

    public void DealDamage(int value)
    {
        if (value > block)
        {
            value -= block;
            block = 0;
            if (has(wStatusEffect.vulernable))
                value *= 2;
            health -= value;
        } else
        {
            block -= value;
        }
        if (health <= 0f)
            onDeathAction.Invoke(this);
        UpdateUI.Invoke();
    }

    public void Heal(int value)
    {
        health = (int)Mathf.Min(maxHealth, health + value);
        UpdateUI.Invoke();
    }

    internal void ApplyStatus(wStatusEffect status, int amount)
    {
        if (statusEffects.ContainsKey(status))
            statusEffects[status].ApplyStacks(amount);
        else
            statusEffects.Add(status, new StatusData(this, status, amount));
        StatusUpdate.Invoke();
    }

    public void AddBlock(int value)
    {
        block += value;
        UpdateUI.Invoke();
    }

    public void EndTurn()
    {
        block = 0;
        bool updatestatus = false;
        var removeList = new List<wStatusEffect>();
        foreach (var kvp in statusEffects)
        {
            kvp.Value.ProcessEOT();
            if (kvp.Value.shouldExpire)
                removeList.Add(kvp.Key);
            updatestatus = true;
        }
        foreach (var s in removeList)
            statusEffects.Remove(s);
        if (updatestatus)
            StatusUpdate.Invoke();
        UpdateUI.Invoke();
    }

    public void SetDeathAction(UnityAction<bEntity> action)
    {
        this.onDeathAction = action;
    }

    public class StatusData
    {
        public bool shouldExpire;
        public int amount;
        wStatusEffect status;
        bEntity owner;

        public StatusData(bEntity owner, wStatusEffect status, int amount)
        {
            this.owner = owner;
            this.amount = amount;
            this.status = status;
        }

        public void ApplyStacks(int n)
        {
            amount += n;
        }

        public void ProcessEOT()
        {
            switch (status)
            {
                case wStatusEffect.weak:
                case wStatusEffect.vulernable:
                case wStatusEffect.rage:
                case wStatusEffect.slow:
                    amount--;
                    if (amount <= 0)
                        shouldExpire = true;
                    break;
                case wStatusEffect.poison:
                    ProcessPoison();
                    break;
                case wStatusEffect.doom:
                case wStatusEffect.protect:
                    break;
            }
        }

        void ProcessPoison()
        {
            owner.DealDamage(amount);
            amount--;
            if (amount <= 0)
                shouldExpire = true;
        }
    }
}
