using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TileController : MonoBehaviour
{
    public SpriteRenderer TileImage;
    public SpriteRenderer HighlightImage;
    public Sprite Possible, Impossibru;

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
    Animation anim;
    TMP_Text valueText;
    Bmap.TileEvents tileEvents;

    public void Init(int id_, int x_, int y_, Bmap bmap)
    {
        valueText = GetComponentInChildren<TextMeshPro>();
        anim = GetComponent<Animation>();

        TileController[,] map = bmap.grid;
        tileSet = bmap.Tileset;
        tileEvents = bmap.tileEvents;

        x = x_;
        y = y_;
        id = id_;
        TileImage.sprite = tileSet[id].sprite;

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

    public void Highlight(bool positive)
    {
        HighlightImage.sprite = (positive) ? Possible : Impossibru;
        HighlightImage.gameObject.SetActive(true);
    }

    public void HideHighlight()
    {
        HighlightImage.gameObject.SetActive(false);
    }

    public void SetTileId(int id_)
    {
        id = id_;
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

    public IEnumerator PlayChange(int id)
    {
        anim.Play("SelectAnim");
        //swap mid animation
        yield return new WaitForSeconds(.15f);
        TileImage.sprite = tileSet[id].sprite;
        while (anim.isPlaying)
            yield return null;
    }

    public void PlayExploitTile(int id = -1)
    {
        anim.Stop();
        anim.Play("ExploitTile");
        if (id != -1)
            TileImage.sprite = tileSet[id].sprite;
    }

    public IEnumerator PlayFail()
    {
        anim.Stop();
        anim.Play("FailSelect");
        while (anim.isPlaying)
            yield return null;
    }
}
