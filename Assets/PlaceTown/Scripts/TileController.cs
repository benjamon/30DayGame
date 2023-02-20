using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TileController : MonoBehaviour
{
    [HideInInspector]
    [System.NonSerialized]
    public int id;
    [HideInInspector]
    [System.NonSerialized]
    public TileController[,] surr = new TileController[3, 3];
    [HideInInspector]
    [System.NonSerialized]
    public int x;
    [HideInInspector]
    [System.NonSerialized]
    public int y;

    Tileset tileSet;
    SpriteRenderer spriteRenderer;
    Animation anim;
    TMP_Text valueText;
    Bmap.TileEvents tileEvents;

    public void Init(int id_, int x_, int y_, Bmap bmap)
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        valueText = GetComponentInChildren<TextMeshPro>();
        anim = GetComponent<Animation>();

        TileController[,] map = bmap.grid;
        tileSet = bmap.Tileset;
        tileEvents = bmap.tileEvents;

        x = x_;
        y = y_;
        id = id_;
        SetTileId(id);

        for (int dx = -1; dx < 2; dx++)
        {
            if ((dx == -1 && x == 0) || (dx == 1 && x == map.GetLength(0)-1))
                continue;
            for (int dy = -1; dy < 2; dy++)
            {
                if ((dx == 0 && dy == 0) || (dy == -1 && y == 0) || (dy == 1 && y == map.GetLength(1) - 1))
                    continue;
                surr[dx+1, dy+1] = map[x + dx, y + dy];
            }
        }
    }

    internal void ShowValue(int id)
    {
        valueText.gameObject.SetActive(true);
        valueText.text = ((id > 0) ? "+" : "") + id;
    }

    internal void HideValue()
    {
        valueText.gameObject.SetActive(false);
    }

    public void SetTileId(int id_)
    {
        id = id_;
        spriteRenderer.sprite = tileSet[id].sprite;
    }

    private void OnMouseDown()
    {
        tileEvents.OnPressed.Invoke(this);
    }

    private void OnMouseEnter()
    {
        tileEvents.OnHovered.Invoke(this);
    }

    internal AudioClip GetClip()
    {
        return tileSet[id].placementSound;
    }

    public bool changeLock { get; private set; }

    public IEnumerator TryChange(int id)
    {
        if (changeLock)
            yield break;
        changeLock = true;
        anim.Play("SelectAnim");
        yield return new WaitForSeconds(.15f);
        SetTileId(id);
        while (anim.isPlaying)
            yield return null;
        changeLock = false;
    }

    public void PlayExploitTile()
    {
        anim.Stop();
        anim.Play("ExploitTile");
    }

    public IEnumerator PlayFail()
    {
        anim.Stop();
        anim.Play("FailSelect");
        while (anim.isPlaying)
            yield return null;
    }
}
