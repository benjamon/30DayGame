using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaceTown : MonoBehaviour
{
    public Bmap Tilemap;
    public TMP_Text ScoreDisplay;
    public AudioSource FailBeep;
    Animation ScoreAnim;
    int score;

    private void Awake()
    {
        ScoreAnim = ScoreDisplay.transform.parent.GetComponent<Animation>();
        Tilemap.tileEvents.OnPressed.AddListener(ClickTile);
        Tilemap.tileEvents.OnHovered.AddListener(HoverTile);
    }

    private void HoverTile(TileController tile)
    {
        var surr = tile.surr;
        Tilemap.HideValues();
        for (int x = 0; x < surr.GetLength(0); x++)
        {
            for (int y =0; y < surr.GetLength(1); y++)
            {
                if (surr[x, y] == null)
                    continue;
                surr[x, y].ShowValue(surr[x, y].id);
            }
        }
    }

    private void ClickTile(TileController tile)
    {
        if (!tile.changeLock)
            Sequencer.MainThread.Enqueue(ProcessPress, tile);
    }

    IEnumerator ProcessPress(TileController tile)
    {
        Debug.Log("clicked " + tile.x + tile.y);
        if (tile.id != 1)
        {
            FailBeep.Stop();
            FailBeep.Play();
            tile.PlayFail();
            yield return new WaitForSeconds(.3f);
            yield break;
        }
        Tilemap.HideValues();
        var surr = tile.surr;
        for (int x = 0; x < surr.GetLength(0); x++)
        {
            for (int y = 0; y < surr.GetLength(1); y++)
            {
                if (surr[x, y] == null)
                    continue;
                score += surr[x, y].id;
            }
        }
        ScoreAnim.Stop();
        ScoreAnim.Play();
        ScoreDisplay.text = score.ToString();
        yield return StartCoroutine(tile.TryChange(0));
    }
}
