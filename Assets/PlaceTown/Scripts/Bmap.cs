using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bmap : MonoBehaviour
{
    public GameObject TilePrefab;

    public Texture2D BaseMap;

    public Tileset Tileset;

    [System.NonSerialized]
    [HideInInspector]
    public TileEvents tileEvents = new TileEvents();
    [System.NonSerialized]
    [HideInInspector]
    public TileController[,] grid;

    public int width;
    public int height;

    private void Awake()
    {
        width = BaseMap.width;
        height = BaseMap.height;

        grid = new TileController[width, height];

        Dictionary<Color32, TileDef> TilesetDict = new Dictionary<Color32, TileDef>();
        for(int i = 0; i < Tileset.tiles.Length; i++)
        {
            Tileset[i].id = i;
            TilesetDict.Add(Tileset[i].color, Tileset[i]);
        }

        Color32[] colors = BaseMap.GetPixels32();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color32 c = colors[x + y * width];
                GameObject g = GameObject.Instantiate(TilePrefab);
                g.transform.position = new Vector3(x, y, 0f);
                grid[x, y] = g.GetComponent<TileController>();
            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color32 c = colors[x + y * width];
                TileDef td = (TilesetDict.ContainsKey(c) ? TilesetDict[c] : Tileset[0]);
                grid[x, y].Init(td.id, x, y, this);
            }
        }
        HideValues();

        Camera.main.transform.position = new Vector3(width / 2f - .5f, height / 2f - .5f, -10f);
        Camera.main.orthographicSize = width / 2f;
    }

    public void ResetMap()
    {
        Dictionary<Color32, TileDef> TilesetDict = new Dictionary<Color32, TileDef>();
        for (int i = 0; i < Tileset.tiles.Length; i++)
        {
            Tileset[i].id = i;
            TilesetDict.Add(Tileset[i].color, Tileset[i]);
        }

        Color32[] colors = BaseMap.GetPixels32();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color32 c = colors[x + y * width];
                TileDef td = (TilesetDict.ContainsKey(c) ? TilesetDict[c] : Tileset[0]);
                grid[x, y].SetTileId(td.id);
            }
        }
    }

    public void HideValues()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].HideValue();
            }
        }
    }

    public int[,] GetBoardState()
    {
        int[,] board = new int[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                board[x, y] = grid[x, y].id;
        return board;
    }

    public class TileEvents
    {
        [System.NonSerialized]
        public TileEvent OnPressed = new TileEvent();
        [System.NonSerialized]
        public TileEvent OnHovered = new TileEvent();
    }

    [System.Serializable]
    public class TileEvent : UnityEvent<TileController> { }
}
