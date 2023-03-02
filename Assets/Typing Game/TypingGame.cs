using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypingGame : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public TextAsset dictionary;
    public TMP_Text playerText;

    Dictionary<string, EnemyDisplay> active = new Dictionary<string, EnemyDisplay>();

    WordProvider words;

    public void Start()
    {
        words = new WordProvider("dictionary.txt", 12);
        StartCoroutine(LogWords());
    }

    string inpt = "";
    float backPressed = 0f;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            for (int i = 97; i < 97+26; i++)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                    inpt = "";
                if (Input.GetKeyDown(KeyCode.Backspace) && backPressed - Time.time < -.05f)
                {
                    inpt = (inpt.Length > 0) ? inpt.Substring(0, inpt.Length - 1) : "";
                    backPressed = Time.time;
                }
                if (Input.GetKeyDown((KeyCode)i))
                {
                    inpt = inpt + ((KeyCode)i).ToString().ToLower();
                    if (active.ContainsKey(inpt))
                    {
                        active[inpt].Kill();
                        active.Remove(inpt);
                        inpt = "";
                    }
                }
            }
            playerText.text = inpt;
        }
    }

    IEnumerator LogWords()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            StartCoroutine(SpawnEnemy());
            yield return new WaitForSeconds(.15f);
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        string w = words.GetRandom(5).ToLower();
        while (active.ContainsKey(w)) w = words.GetRandom(5).ToLower();
        var go = GameObject.Instantiate(EnemyPrefab);
        var ed = go.GetComponent<EnemyDisplay>();
        active.Add(w, ed);
        go.transform.position = new Vector3(Random.Range(-3f, 3f), Random.Range(-5f, 5f), 0f);
        ed.SetWord(w);
        yield return new WaitForSeconds(4f);
        ed.Destroy();
        if (active.ContainsKey(w))
            active.Remove(w);
    }
}
