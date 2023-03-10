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

    public EnemyDef enemyConfig;

    public AudioSource basic;
    public AudioSource turnStart;
    public AudioSource turnEnd;

    public BattleStatus status { get; private set; }

    private void Start()
    {
        bEntity hero = new bEntity(10, LogDeath);
        bEntity enemy = new bEntity(50, LogDeath);
        enemyConfig.Setup(this.enemy);
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
        yield return new WaitForSeconds(2f);
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