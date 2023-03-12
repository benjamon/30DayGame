using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TargetWord : MonoBehaviour
{
    public TMP_Text WordText;
    public TMP_Text CompletedText;
    public string word { get; private set; }
    public int completion { get; private set; }
    public UnityEvent OnComplete = new UnityEvent();
    public UnityEvent OnTyped = new UnityEvent();

    public void Init(string word)
    {
        WordText.text = word;
        CompletedText.text = "";
        completion = 0;
        WordText.color = new Color(0f, 0f, 0f, 1f);
        this.word = word;
    }

    public void ShowLength(int n)
    {
        CompletedText.text = "";
        if (n <= 3)
            WordText.text = $"(s)";
        else if (n <= 6)
            WordText.text = $"(m)";
        else if (n <= 8)
            WordText.text = $"(l)";
        else
            WordText.text = $"(xl)";
    }

    public void ProcessCharacter(char c)
    {
        if (completion >= word.Length)
        {
            OnComplete.Invoke();
            return;
        } else if (word[completion] == c)
        {
            completion++;
            CompletedText.text = word.Substring(0, completion);
            OnTyped.Invoke();
            if (completion < word.Length)
            {
                WordText.text = word.Substring(0, completion) + "<b><color=\"yellow\">" + word[completion] + "</color></b>" + word.Substring(completion + 1, word.Length - completion - 1);
            }
        }
        if (completion >= word.Length)
        {
            OnComplete.Invoke();
        }
    }
}
