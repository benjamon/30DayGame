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

    public TileRule[] rules;

    public int placeOn = -1;

    public TILE_FLAGS placeOnFlags = TILE_FLAGS.OPEN;

    public TILE_FLAGS flags;

    public bool CheckPlaceable(int id, TileDef def)
    {
        return (placeOn == -1) ? (def.flags & placeOnFlags) != 0 : id == placeOn;
    }
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

[System.Serializable]
public class TileRule
{
    public int id;
    public int value;
}