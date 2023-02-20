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
        {
            Tilemap.ResetMap();
            score = 0;
            ScoreDisplay.text = score.ToString();
        }
    }

    private void HoverTile(TileController tile)
    {
        var surr = tile.surr;
        Tilemap.HideValues();
        TileRule[] rules = Tilemap.Tileset[CurrentCard].rules;
        for (int x = 0; x < surr.GetLength(0); x++)
        {
            for (int y =0; y < surr.GetLength(1); y++)
            {
                if (surr[x, y] == null)
                    continue;
                int sum = 0;
                int id = surr[x, y].id;
                for (int i = 0; i < rules.Length; i++)
                    if (id == rules[i].id)
                        sum += rules[i].value;
                if (sum == 0)
                {
                    surr[x, y].HideValue();
                } else
                {
                    surr[x, y].ShowValue(sum);
                }
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
        TileRule[] rules = Tilemap.Tileset[CurrentCard].rules;
        if (rules.Length == 0)
            return 0;
        for (int x = 0; x < surr.GetLength(0); x++)
        {
            for (int y = 0; y < surr.GetLength(1); y++)
            {
                if (surr[x, y] == null)
                    continue;
                int id = surr[x, y].id;
                for (int i = 0; i < rules.Length; i++)
                    if (id == rules[i].id)
                        sum += rules[i].value;
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
