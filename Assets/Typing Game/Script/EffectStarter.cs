using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        var au = GetComponentInChildren<AudioSource>();
        if (au != null)
        { 
            au.Stop();
            au.Play();
        }
        var an = GetComponentInChildren<Animation>();
        
        if (an != null)
        {
            Debug.Log("HIT");
            an.Stop();
            an.Rewind();
            an.Play();
        }
    }
}
