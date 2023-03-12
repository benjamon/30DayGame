using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordBattle : MonoBehaviour
{
    public EntityActor hero;
    public EntityActor enemy;

    public TypeTheSpire game;

    public TimerDisplay Timer;

    public float turnLength = 10f;

    public EnemyDef[] enemyConfig;

    public AudioSource basic;
    public AudioSource turnStart;
    public AudioSource turnEnd;

    public Bentendo.typeSpire.ShopUI shop;

    public BattleStatus status { get; private set; }
    int round = 0;

    private void Start()
    {
        SetupBattle();
    }

    public void SetupBattle()
    {
        var config = enemyConfig[UnityEngine.Random.Range(0, enemyConfig.Length)];
        bEntity hero = new bEntity(6, LogDeath);
        bEntity enemy = new bEntity(config.hp + round * 3, LogDeath);
        config.Setup(this.enemy);
        round++;
        StartBattle(hero, enemy);
    }

    void LogDeath(bEntity e)
    {
        Debug.Log("AN ENTITY IS DEATH WITH MAX HP " + e.maxHealth);
    }

    public void StartBattle(bEntity hero, bEntity enemy)
    {
        this.hero.Setup(hero);
        this.enemy.Setup(enemy);
        StartCoroutine(StartSequence());
        TypeTheSpire.Instance.DefineEntities(hero, enemy);
    }
    
    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(shop.WaitShopChoice());
        yield return new WaitForSeconds(1f);
        StartCoroutine(StartTurn());
    }

    private IEnumerator StartTurn()
    {
        TypeTheSpire.Instance.DefineEntities(hero.entity, enemy.entity);
        var intent = enemy.GetNext();
        enemy.ShowIntent(intent);
        game.DrawCards();
        basic.Play();
        Timer.SetTime(3f);
        yield return new WaitForSeconds(2f);
        basic.Play();
        Timer.SetTime(2f);
        yield return new WaitForSeconds(2f);
        basic.Play();
        Timer.SetTime(1f);
        yield return new WaitForSeconds(2f);
        turnStart.Play();

        float t = Time.time;
        status = BattleStatus.InProgress;
        game.ShowHand();
        while (Time.time - t < turnLength)
        {
            float tl = turnLength - (Time.time - t);
            Timer.SetTime(tl);
            yield return null;
        }
        TypeTheSpire.Instance.DiscardHand();
        turnEnd.Play();
        Timer.SetTime(0f);
        status = BattleStatus.Between;
        enemy.entity.EndTurn();
        if (enemy.entity.health <= 0)
        {
            SetupBattle();
            yield break;
        }
        intent.spell.action.InvokeOn(enemy.entity, hero.entity);
        yield return new WaitForSeconds(2f);
        hero.entity.EndTurn();
        StartCoroutine(StartTurn());
    }
}

public enum BattleStatus
{
    Between,
    InProgress,
    Won,
    Lost,
}