using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    public TMP_Text clock;

    private void Start()
    {
        clock.text = "";
    }

    public void SetTime(float time)
    {
        if (time < 2f)
        {
            time = Mathf.Round(time * 10f) / 10f;
        }
        else
            time = Mathf.Round(time);
        clock.text = time.ToString();
    }
}
