using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypingGame : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public TextAsset dictionary;
    public TMP_Text playerText;

    TouchScreenKeyboard m_Keyboard;

    Dictionary<string, EnemyDisplay> active = new Dictionary<string, EnemyDisplay>();

    WordProvider words;

    int min = 3;
    int max = 8;
    int len = 4;

    public void Start()
    {
        Application.targetFrameRate = 45;
        words = new WordProvider("dictionary.txt", max);
        Sequencer.main.WaitUntil(words.IsSetup);
        Sequencer.main.Enqueue(LogWords);
    }

    string inpt = "";
    float backPressed = 0f;

    private void Update()
    {
        if (m_Keyboard == null || !m_Keyboard.active || m_Keyboard.status != TouchScreenKeyboard.Status.Visible)
        {
            TouchScreenKeyboard.hideInput = false;
            m_Keyboard = TouchScreenKeyboard.Open(inpt, TouchScreenKeyboardType.ASCIICapable, false, false, true);
            TouchScreenKeyboard.hideInput = false;
            TouchScreenKeyboard.Android.consumesOutsideTouches = false;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            len = Mathf.FloorToInt(Mathf.Min(max, min + Time.time / 15f));
            if (m_Keyboard.text.Length > len)
            {
                m_Keyboard.text = m_Keyboard.text.Substring(m_Keyboard.text.Length - len, len);
            }
            string txt = m_Keyboard.text;
            playerText.text = txt;
            if (active.ContainsKey(txt))
            {
                active[txt].Kill();
                active.Remove(txt);
                m_Keyboard.text = "";
            }
        } else
        {
            len = Mathf.FloorToInt(Mathf.Min(max, min + Time.time / 10f));
            if (Input.anyKeyDown)
            {
                for (int i = 97; i < 97 + 26; i++)
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
    }

    IEnumerator LogWords()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.15f);
            StartCoroutine(SpawnEnemy());
            yield return new WaitForSeconds(3f);
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        int l = Random.Range(min, len+1);
        string w = words.GetRandom(l).ToLower();
        while (active.ContainsKey(w)) w = words.GetRandom(l).ToLower();
        var go = GameObject.Instantiate(EnemyPrefab);
        var ed = go.GetComponent<EnemyDisplay>();
        active.Add(w, ed);
        go.transform.position = new Vector3(Random.Range(-3f, 3f), Random.Range(4f, 6f), 0f);
        ed.SetWord(w);
        yield return new WaitForSeconds(4f);
        ed.Destroy();
        if (active.ContainsKey(w))
            active.Remove(w);
    }
}
