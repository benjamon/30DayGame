using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bmap : MonoBehaviour
{
    public GameObject TilePrefab;

    public Texture2D BaseMap;

    public TileDef[] TileSet;

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

        Dictionary<Color32, TileDef> tileSetDict = new Dictionary<Color32, TileDef>();
        for(int i = 0; i < TileSet.Length; i++)
        {
            TileSet[i].id = i;
            tileSetDict.Add(TileSet[i].color, TileSet[i]);
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
                TileDef td = (tileSetDict.ContainsKey(c) ? tileSetDict[c] : TileSet[0]);
                grid[x, y].Init(td.id, x, y, this);
            }
        }
        HideValues();

        Camera.main.transform.position = new Vector3(width / 2f - .5f, height / 2f - .5f, -10f);
        Camera.main.orthographicSize = width / 2f;
    }

    public void ResetMap()
    {
        Dictionary<Color32, TileDef> tileSetDict = new Dictionary<Color32, TileDef>();
        for (int i = 0; i < TileSet.Length; i++)
        {
            TileSet[i].id = i;
            tileSetDict.Add(TileSet[i].color, TileSet[i]);
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
                TileDef td = (tileSetDict.ContainsKey(c) ? tileSetDict[c] : TileSet[0]);
                grid[x, y].Init(td.id, x, y, this);
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
    
    [System.Serializable]
    public class TileDef
    {
        public string name;
        public Color32 color;
        public Sprite sprite;
        [System.NonSerialized]
        public int id;
        public AudioClip placementSound;
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
