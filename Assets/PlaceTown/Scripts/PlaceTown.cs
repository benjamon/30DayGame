using System.Collections;
using TMPro;
using UnityEngine;

public class PlaceTown : MonoBehaviour
{
    public const int MAX_QUEUED = 2;

    public Bmap Tilemap;
    public TMP_Text ScoreDisplay;
    public TMP_Text ScoreChange;
    public AudioSource FailSource;
    public AudioSource SuccessSource;
    public int[] DefaultDeck;
    public DeckUI DeckUI;
    Deck<int> playerDeck;
    [System.NonSerialized]
    [HideInInspector]
    public int CurrentCard;

    int queueCount = 0;
    
    AudioClip defaultPlacementClip;
    Animation ScoreAnim;
    int score;

    private void Awake()
    {
        ScoreAnim = ScoreDisplay.transform.parent.GetComponent<Animation>();
        Tilemap.tileEvents.OnPressed.AddListener(ClickTile);
        Tilemap.tileEvents.OnHovered.AddListener(HoverTile);
        defaultPlacementClip = SuccessSource.clip;
        playerDeck = new Deck<int>(DefaultDeck);
        playerDeck.TryDrawNext(out CurrentCard);
        DeckUI.Init(playerDeck, Tilemap.Tileset, this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Tilemap.ResetMap();
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
        if (queueCount < MAX_QUEUED && !tile.changeLock)
        {
            Sequencer.MainThread.Enqueue(ProcessPress, tile);
            queueCount++;
        }
    }

    void DrawNext()
    {
        playerDeck.AddToDiscard(CurrentCard);
        if (playerDeck.TryDrawNext(out int newCard))
            CurrentCard = newCard;
    }

    IEnumerator ProcessPress(TileController tile)
    {
        if ((Tilemap.Tileset[tile.id].flags & TILE_FLAGS.OPEN) == 0)
        {
            FailSource.Stop();
            FailSource.Play();
            yield return StartCoroutine(tile.PlayFail());
            queueCount--;
            yield break;
        }
        SuccessSource.Stop();
        AudioClip clip = Tilemap.Tileset.tiles[CurrentCard].placementSound;
        SuccessSource.clip = (clip) ? clip : defaultPlacementClip;
        SuccessSource.Play();
        int diff = CalculateValue(tile);
        ScoreChange.text = (diff > 0) ? "+" + diff : diff.ToString();
        score += diff;
        ScoreDisplay.text = score.ToString();
        ScoreAnim.Stop();
        ScoreAnim.Play();
        StartCoroutine(ExploitTile(tile));
        yield return StartCoroutine(tile.TryChange(CurrentCard));
        if (playerDeck.DrawPileEmpty())
            yield return StartCoroutine(playerDeck.ShuffleRoutine(DeckUI.PlayShuffle, .15f));
        DrawNext();
        yield return StartCoroutine(DeckUI.UpdateUI());
        queueCount--;
    }

    public int CalculateValue(TileController tile)
    {
        int sum = 0;
        var surr = tile.surr;
        for (int x = 0; x < surr.GetLength(0); x++)
        {
            for (int y = 0; y < surr.GetLength(1); y++)
            {
                if (surr[x, y] == null)
                    continue;
                sum += surr[x, y].id;
            }
        }
        return sum;
    }
    public IEnumerator ExploitTile(TileController tile)
    {
        int sum = 0;
        var surr = tile.surr;
        for (int x = 0; x < surr.GetLength(0); x++)
        {
            for (int y = 0; y < surr.GetLength(1); y++)
            {
                if (surr[x, y] == null)
                    continue;
                surr[x,y].PlayExploitTile();
                yield return new WaitForSeconds(.04f);
            }
        }
    }
}
