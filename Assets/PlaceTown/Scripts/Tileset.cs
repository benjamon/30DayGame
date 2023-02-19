using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tileset : ScriptableObject
{
    public TileDef[] tiles;
}


public class TileDef
{
    public string name;
    public Color32 color;
    public Sprite sprite;
    [System.NonSerialized]
    public int id;
    public AudioClip placementSound;
}