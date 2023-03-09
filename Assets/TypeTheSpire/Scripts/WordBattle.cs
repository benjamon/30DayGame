using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordBattle : MonoBehaviour
{
    public EntityActor hero;
    public EntityActor enemy;

    public TypeTheSpire game;

    public TMP_Text Timer;

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
        Timer.gameObject.SetActive(false);
    }

    void LogDeath(bEntity e)
    {
        Debug.Log("AN ENTITY IS DEATH WITH MAX HP " + e.maxHealth);
    }

    public void StartBattle(bEntity hero, bEntity enemy)
    {
        this.hero.Setup(hero);
        this.enemy.Setup(enemy);
        StartCoroutine(StartTurn());
        TypeTheSpire.Instance.DefineEntities(hero, enemy);
    }

    private IEnumerator StartTurn()
    {
        TypeTheSpire.Instance.DefineEntities(hero.entity, enemy.entity);
        var intent = enemy.GetNext();
        enemy.ShowIntent(intent);
        basic.Play();
        yield return new WaitForSeconds(1f);
        basic.Play();
        yield return new WaitForSeconds(1f);
        basic.Play();
        yield return new WaitForSeconds(1f);
        turnStart.Play();

        float t = Time.time;
        Timer.text = turnLength.ToString();
        Timer.gameObject.SetActive(true);
        status = BattleStatus.InProgress;
        game.DrawCards();
        while (Time.time - t < turnLength)
        {
            float tl = turnLength - (Time.time - t);
            if (tl < 2f)
            {
                tl = Mathf.Round(tl * 10f) / 10f;
            }
            else
                tl = Mathf.Round(tl);
            Timer.text = tl.ToString();
            yield return null;
        }
        TypeTheSpire.Instance.DiscardHand();
        turnEnd.Play();
        Timer.gameObject.SetActive(false);
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