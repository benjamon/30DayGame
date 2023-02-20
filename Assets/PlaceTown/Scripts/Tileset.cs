using UnityEngine;

[CreateAssetMenu(fileName = "tileset", menuName = "new tileset", order = 0)]
public class Tileset : ScriptableObject
{
    public TileDef[] tiles;

    public TileDef this[int n] => tiles[n];
}


[System.Serializable]
public class TileDef
{
    public string name;
    public Color32 color;
    public Sprite sprite;
    [System.NonSerialized]
    public int id;
    public AudioClip placementSound;

    [System.NonSerialized]
    public TileRule[] rules;

    public TILE_FLAGS flags;
}

[System.Flags]
public enum TILE_FLAGS {
    OPEN = 1,
    BUILDING = 2,
    RESOURCE = 4,
    INDUSTRY = 8,
    CIVILIAN = 16,
    COMMERCE = 32,
    BLESSING = 64,
    HAZARD = 128
}

public class TileRule
{
    public int id;
    public int value;
}