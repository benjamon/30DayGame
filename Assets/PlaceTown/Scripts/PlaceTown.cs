using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public ShopUI ShopUI;
    Deck<int> playerDeck;
    [System.NonSerialized]
    [HideInInspector]
    public int CurrentCard;

    int queueCount = 0;
    
    AudioClip defaultPlacementClip;
    Animation ScoreAnim;
    Sequencer sequencer;
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
        ShopUI.Init(this, Tilemap.Tileset);
    }

    private void Start()
    {
        sequencer = Sequencer.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.SetActiveScene(SceneManager.GetActiveScene());
        }
    }

    private void HoverTile(TileController tile)
    {
        var surr = tile.surr;
        Tilemap.HideValues();
        if (!Tilemap.Tileset[CurrentCard].CheckPlaceable(tile.id, Tilemap.Tileset[tile.id]))
            return;
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

    bool shopLock = false;

    IEnumerator WaitPurchase()
    {
        ShopUI.gameObject.SetActive(true);
        ShopUI.RandomOptions();
        shopLock = true;
        while (shopLock)
            yield return null;
    }

    public void PurchaseCardFromShop(int n)
    {
        playerDeck.AddToDiscard(n);
        shopLock = false;
        ShopUI.gameObject.SetActive(false);
    }

    private void ClickTile(TileController tile)
    {
        if (queueCount < MAX_QUEUED && !tile.changeLock && !shopLock)
        {
            sequencer.Enqueue(ProcessPress, tile);
            queueCount++;
        }
    }

    void DrawNext()
    {
        if (playerDeck.TryDrawNext(out int newCard))
            CurrentCard = newCard;
    }

    IEnumerator ProcessPress(TileController tile)
    {
        if (!Tilemap.Tileset[CurrentCard].CheckPlaceable(tile.id, Tilemap.Tileset[tile.id]))
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
        playerDeck.AddToDiscard(CurrentCard);
        if (playerDeck.DrawPileEmpty())
        {
            yield return StartCoroutine(WaitPurchase());
            yield return StartCoroutine(playerDeck.ShuffleRoutine(DeckUI.PlayShuffle, .15f));
        }
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
