using UnityEngine;

[CreateAssetMenu(fileName = "tileset", menuName = "new tileset", order = 0)]
public class Tileset : ScriptableObject
{
    public TileDef[] tiles;

    public TileDef this[int n] => tiles[n];
}


[System.Serializable]
public class TileDef : tiledata
{
    public int id { get; private set; }

    public string name;
    public Color32 color;
    public Sprite sprite;
    public AudioClip placementSound;

    public TileRule[] rules;
    public TileAction[] actions;

    public int placeOn = -1;

    public TILE_FLAGS placeOnFlags = TILE_FLAGS.OPEN;

    public TILE_FLAGS flags;

    public bool CheckPlaceable(int id, TileDef def)
    {
        return (placeOn == -1) ? (def.flags & placeOnFlags) != 0 : id == placeOn;
    }

    public int getid()
    {
        return id;
    }

    public void setid(int id)
    {
        this.id = id;
    }

    internal void ApplyActions(TileController[,] surr)
    {
        for (int i = 0; i < actions.Length; i++)
            actions[i].ApplyAction(surr);
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

public enum TileActionType
{ 
    ChangeType,
}

[System.Serializable]
public class TileRule
{
    public int id;
    public int value;
}

[System.Serializable]
public class TileAction
{
    public TileActionType actionType;
    public int[] targetIds;
    public int resultId;
    public bool[,] grid = new bool[3, 3];

    public void ApplyAction(TileController[,] surr)
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (surr[x, y])
                    ApplyRuleToTile(surr[x, y]);
            }
        }
    }

    public void ApplyRuleToTile(TileController tile)
    {
        bool isTarget = false;
        for (int i = 0; i < targetIds.Length; i++)
        {
            if (tile.id == targetIds[i])
            {
                isTarget = true;
                break;
            }
        }
        if (!isTarget)
            return;
        switch (actionType)
        {
            case TileActionType.ChangeType:
                tile.SetTileId(resultId);
                break;
        }
    }
}