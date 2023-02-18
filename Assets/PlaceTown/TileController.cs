using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [HideInInspector]
    [System.NonSerialized]
    public TileData info = new TileData();
    Bmap.TileDef[] tileSet;
    SpriteRenderer spriteRenderer;

    public void Init(int id, int x, int y, TileController[,] map, Bmap.TileDef[] tileSet_)
    {
        info.x = x;
        info.y = y;
        tileSet = tileSet_;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SetTileId(id);

        for (int dx = -1; dx < 2; dx++)
        {
            if ((dx == -1 && x == 0) || (dx == 1 && x == map.GetLength(0)-1))
                continue;
            for (int dy = -1; dy < 2; dy++)
            {
                if ((x == 1 && y == 1) || (dy == -1 && y == 0) || (dy == 1 && y == map.GetLength(1) - 1))
                    continue;
                info.surrounding[dx+1, dy+1] = map[x + dx, y + dy].info;
            }
        }
    }

    public void SetTileId(int id)
    {
        spriteRenderer.sprite = tileSet[id].sprite;
        info.index = id;
    }

    private void OnMouseDown()
    {
        if (!locked)
            StartCoroutine(Change((info.index == 0) ? 1 : 0));
    }

    bool locked;

    IEnumerator Change(int id)
    {
        locked = true;
        float t = Time.time;
        float dur = .5f;
        GetComponent<Animation>().Play();
        while (Time.time - t < dur)
        {
            float norm = (Time.time - t) / dur;
            norm = norm * norm;
            transform.rotation = Quaternion.identity;
            transform.Rotate(Vector3.forward, norm * 360f);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.identity;
        SetTileId(id);
        locked = false;
    }

    public class TileData
    {
        public int index;
        public TileData[,] surrounding = new TileData[3,3];
        public int x;
        public int y;
    }
}
