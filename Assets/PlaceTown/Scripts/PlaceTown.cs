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
    Tileset tileSet;
    int score;

    private void Awake()
    {
        ScoreAnim = ScoreDisplay.transform.parent.GetComponent<Animation>();
        Tilemap.tileEvents.OnPressed.AddListener(ClickTile);
        Tilemap.tileEvents.OnHovered.AddListener(HoverTile);
        tileSet = Tilemap.Tileset;
        defaultPlacementClip = SuccessSource.clip;
        playerDeck = new Deck<int>(DefaultDeck);
        playerDeck.TryDrawNext(out CurrentCard);
        DeckUI.Init(playerDeck, tileSet, this);
        ShopUI.Init(this, tileSet);
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
        if (!tileSet[CurrentCard].CheckPlaceable(tile.id, tileSet[tile.id]))
            return;
        TileRule[] rules = tileSet[CurrentCard].rules;
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
        if (queueCount < MAX_QUEUED && !shopLock)
        {
            if (!Tilemap.Tileset[CurrentCard].CheckPlaceable(tile.id, Tilemap.Tileset[tile.id]))
            {
                sequencer.Enqueue(FailAnim, tile);
            } else
            {
                var value = CalculateValue(tile, CurrentCard);
                var move = new MoveData(Tilemap, tile, tileSet[tile.id], tileSet[CurrentCard], score, value);
                score += value;

                playerDeck.AddToDiscard(CurrentCard);
                sequencer.Enqueue(PlayMoveSequence, move);

                if (playerDeck.DrawPile.Count == 0)
                {
                    shopLock = true;
                    sequencer.Enqueue(WaitPurchase);
                    sequencer.Enqueue(ShuffleDeck);
                } else
                {
                    DrawNext();
                }

            }
            queueCount++;
        }
    }

    public IEnumerator ShuffleDeck()
    {
        playerDeck.Shuffle();
        var topCard = tileSet[playerDeck.DrawPile.Peek()].sprite;
        sequencer.Enqueue(DrawNext);
        yield return StartCoroutine(DeckUI.PlayShuffleRoutine(topCard));
    }

    IEnumerator FailAnim(TileController tile)
    {
        FailSource.Stop();
        FailSource.Play();
        yield return StartCoroutine(tile.PlayFail());
        queueCount--;
    }

    void DrawNext()
    {
        if (playerDeck.TryDrawNext(out int newCard))
        {
            CurrentCard = newCard;

            Sprite card = null;
            if (playerDeck.TryPeekNext(out int newDraw))
                card = tileSet[newDraw].sprite;
            sequencer.Enqueue(DeckUI.PlayDrawRoutine, card);
        }
        else
            Debug.LogError("no card found");
    }

    IEnumerator PlayMoveSequence(MoveData move)
    {
        int playedId = move.cardPlayed.id;
        SuccessSource.Stop();
        AudioClip clip = Tilemap.Tileset.tiles[playedId].placementSound;
        SuccessSource.clip = (clip) ? clip : defaultPlacementClip;
        SuccessSource.Play();
        int reward = move.moveScore;
        ScoreChange.text = (reward > 0) ? "+" + reward : reward.ToString();
        ScoreDisplay.text = move.score.ToString();
        ScoreAnim.Stop();
        ScoreAnim.Play();
        yield return StartCoroutine(move.tile.PlayChange(playedId));
        yield return StartCoroutine(ExploitTile(move));
        queueCount--;
    }

    public int CalculateValue(TileController tile, int card)
    {
        int sum = 0;
        var surr = tile.surr;
        TileRule[] rules = Tilemap.Tileset[card].rules;
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

    public IEnumerator ExploitTile(MoveData move)
    {
        var surr = move.tile.surr;
        for (int x = 0; x < surr.GetLength(0); x++)
        {
            for (int y = 0; y < surr.GetLength(1); y++)
            {
                
                if (surr[x, y] == null)
                    continue;
                surr[x,y].PlayExploitTile();
                yield return new WaitForSeconds(.05f);
            }
        }
    }
    
    public class MoveData
    {
        public TileController tile;
        public TileDef prevCard;
        public TileDef cardPlayed;
        public int[,] previousBoardState;
        public int score;
        public int moveScore;

        public MoveData(Bmap map_, TileController tile_, TileDef prevState_, TileDef nextState_, int score_, int moveScore_)
        {
            previousBoardState = map_.GetBoardState();
            tile = tile_;
            prevCard = prevState_;
            cardPlayed = nextState_;
            score = score_;
            moveScore = score_;
        }
    }
}
