using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDisplay : MonoBehaviour
{
    public GameObject DeathObject;
    public TMP_Text word;
    public TMP_Text input;
    bool alive = true;

    private void Update()
    {
        transform.position += Time.deltaTime * Vector3.down * ((alive) ? .3f : -22f);
    }

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
        var go = GameObject.Instantiate(DeathObject);
        go.transform.position = transform.position;
        GameObject.Destroy(go, 2f);
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
