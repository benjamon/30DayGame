using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class typeSounds : MonoBehaviour
{
    public AudioSource sound;

    public SpriteRenderer bg;

    public Color highlighted;

    private void Awake()
    {
        GetComponent<TargetWord>().OnTyped.AddListener(Soundoff);
    }

    private void Soundoff()
    {
        sound.Play();
        bg.color = highlighted;
    }
}
