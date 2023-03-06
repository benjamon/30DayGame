using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TypingGame : MonoBehaviour
{
    public GameObject Shell;
    public AudioSource Gunshot;
    public AudioSource Gunclick;
    public TMP_Text playerText;
    public TMP_Text nextWordText;
    TouchScreenKeyboard m_Keyboard;

    WordProvider words;

    int min = 3;
    int max = 8;
    int len = 4;

    string inpt = "";
    string nextWord = "";
    int lastLen = 1;
    bool start;

    public void Start()
    {
        Application.targetFrameRate = 45;
        words = new WordProvider("dictionary.txt", max);
        Sequencer.main.WaitUntil(() => words.IsSetup);
        Sequencer.main.Enqueue(() =>
        {
            StartGame();
        });
    }

    private void StartGame()
    {
        GetNextWord();
        start = true;
    }

    private void LateUpdate()
    {
        if (!start)
            return;
        if (Application.platform == RuntimePlatform.Android)
        {
            if (m_Keyboard == null || !m_Keyboard.active || m_Keyboard.status != TouchScreenKeyboard.Status.Visible)
            {
                inpt = "";
                lastLen = 1;
                TouchScreenKeyboard.hideInput = true;
                m_Keyboard = TouchScreenKeyboard.Open("~", TouchScreenKeyboardType.NamePhonePad, false, false);
                m_Keyboard.selection = new RangeInt(m_Keyboard.text.Length, 0);
                TouchScreenKeyboard.Android.consumesOutsideTouches = false;
            }
            len = Mathf.FloorToInt(Mathf.Min(max, min + Time.time / 14f));
            if (m_Keyboard.text.Length > lastLen)
            {
                string str = m_Keyboard.text.ToLower();
                for (int i = lastLen; i < str.Length; i++)
                {
                    if (inpt.Length >= nextWord.Length)
                    {
                        if (str[i] != ' ')
                            continue;
                        GetNextWord();
                        m_Keyboard.text = "~";
                        lastLen = 1;
                        inpt = "";
                        Fire();
                    }
                    else if (nextWord[inpt.Length] == str[i])
                    {
                        inpt += str[i];
                        playerText.text = inpt;
                        Gunclick.Stop();
                        Gunclick.Play();
                        if (inpt == nextWord)
                            Shell.gameObject.SetActive(true);
                    }
                }
                lastLen = m_Keyboard.text.Length;
            }
        }
        /*
        else
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
                        if (inpt == nextWord)
                        {
                            GetNextWord();
                            Shell.gameObject.SetActive(true);
                            inpt = "";
                        }
                    }
                }
                playerText.text = inpt;
            }
        }
        */
    }

    public void Fire()
    {
        if (!Shell.gameObject.activeSelf)
            return;
        Shell.gameObject.SetActive(false);
        Gunshot.Play();
    }

    private void GetNextWord()
    {
        nextWord = words.GetRandom(UnityEngine.Random.Range(min, len+1)).ToLower();
        nextWordText.text = nextWord;
        playerText.text = "";
    }
}
