using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDisplay : MonoBehaviour
{
    public TMP_Text word;
    public TMP_Text input;
    bool alive = true;

    public void SetWord(string s)
    {
        word.text = s;
        input.text = "";
    }

    public void Kill()
    {
        alive = false;
        word.text = "";
        input.text = "dead";
    }

    public void Destroy()
    {
        if (alive)
        {
            Debug.Log("owie");
        }
        Destroy(gameObject);
    }
}
