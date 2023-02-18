using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bmap : MonoBehaviour
{
    public GameObject TilePrefab;

    public Texture2D BaseMap;

    public TileDef[] TileSet;

    TileController[,] controllers;

    private void Awake()
    {
        int w = BaseMap.width;
        int h = BaseMap.height;

        controllers = new TileController[w, h];

        Dictionary<Color32, TileDef> tileSetDict = new Dictionary<Color32, TileDef>();
        for(int i = 0; i < TileSet.Length; i++)
        {
            TileSet[i].id = i;
            tileSetDict.Add(TileSet[i].color, TileSet[i]);
        }

        Color32[] colors = BaseMap.GetPixels32();

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Color32 c = colors[x + y * w];
                GameObject g = GameObject.Instantiate(TilePrefab);
                g.transform.position = new Vector3(x, y, 0f);
                controllers[x, y] = g.GetComponent<TileController>();
            }
        }
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Color32 c = colors[x + y * w];
                TileDef td = (tileSetDict.ContainsKey(c) ? tileSetDict[c] : TileSet[0]);
                controllers[x, y].Init(td.id, x, y, controllers, TileSet);
            }
        }

        Camera.main.transform.position = new Vector3(w / 2f - .5f, h / 2f - .5f, -10f);
        Camera.main.orthographicSize = w / 2f;
    }
    
    [System.Serializable]
    public class TileDef
    {
        public Color32 color;
        public Sprite sprite;
        [System.NonSerialized]
        public int id;
    }
}
