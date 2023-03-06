using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetWord : MonoBehaviour
{
    public TMP_Text WordText;
    public TMP_Text CompletedText;
    public string word { get; private set; }
    public int completion { get; private set; }

    public void Init(string word)
    {
        Debug.Log("init " + word);
        WordText.text = word;
        CompletedText.text = "";
        completion = 0;
        this.word = word;
    }

    public void ProcessCharacter(char c)
    {
        if (word[completion] == c)
        {
            completion++;
            CompletedText.text = word.Substring(0, completion);
        }
        if (completion >= word.Length)
            CompleteWord();
    }

    public void CompleteWord()
    {
        TypeTheSpire.Instance.WordCompleted(this);
        Debug.Log("word completed");
    }
}
