using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bentendo.Tilemaps;

public class TilemapView : MonoBehaviour
{
    public int width;
    public int height;
    public TesteTileDef[] tiles;
    SpriteRenderer[,] cells;
    int[,] map;

    public void Awake()
    {
        map = new int[width, height];
        cells = new SpriteRenderer[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = Random.Range(0, tiles.Length);
                var go = new GameObject(x + ", " + y + " hehe");
                go.transform.position = new Vector2(-width / 2f + x, -height / 2f + y);
                cells[x, y] = go.AddComponent<SpriteRenderer>();
                cells[x, y].sprite = tiles[map[x, y]].sprite;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = Random.Range(0, tiles.Length);
                }
            }
            DrawMap();
        }
    }

    public void DrawMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y].sprite = tiles[map[x, y]].sprite;
            }
        }
    }
}
