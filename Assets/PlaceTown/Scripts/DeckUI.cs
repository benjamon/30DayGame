using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour
{
    public const string DRAW_ANIM = "DrawCard";
    public const string DISCARD_ANIM = "Discard";
    public const string DISCARD_TO_DRAW_ANIM = "ShuffleBack";

    public TMP_Text DrawPileCount;
    public TMP_Text DiscardCount;
    public Image ActiveCard;
    public Image DrawPile;
    public Image Discard;
    public Image Animated;
    public AudioSource CardTransitionSound;

    PlaceTown town;
    Deck<int> playerDeck;
    Tileset tileSet;
    Sprite emptySprite;
    Animation anim;

    public void Init(Deck<int> deck_, Tileset tileSet_, PlaceTown town_)
    {
        tileSet = tileSet_;
        emptySprite = Discard.sprite;
        town = town_;
        playerDeck = deck_;
        anim = GetComponent<Animation>();
        UpdateInstant();
    }

    public IEnumerator UpdateUI()
    {
        PlayAnim(DISCARD_ANIM);

        ActiveCard.sprite = emptySprite;

        yield return new WaitForSeconds(.2f);

        if (playerDeck.TryGetLastDiscarded(out int dscrd))
            Discard.sprite = tileSet[dscrd].sprite;
        else
            Discard.sprite = emptySprite;

        DiscardCount.text = playerDeck.Discard.Count.ToString();

        PlayAnim(DRAW_ANIM);

        if (playerDeck.TryPeekNext(out int nxtCard))
            DrawPile.sprite = tileSet[nxtCard].sprite;
        else
            DrawPile.sprite = emptySprite;

        yield return new WaitForSeconds(.2f);

        ActiveCard.sprite = tileSet[town.CurrentCard].sprite;

        DrawPileCount.text = playerDeck.DrawPile.Count.ToString();
    }

    void PlayAnim(string title)
    {
        anim.Stop();
        anim.Play(title);

        switch (title)
        {
            case DRAW_ANIM:
                Animated.sprite = DrawPile.sprite;
                break;
            case DISCARD_ANIM:
                Animated.sprite = ActiveCard.sprite;
                break;
            case DISCARD_TO_DRAW_ANIM:
                if (playerDeck.TryGetLastDiscarded(out int last))
                    Animated.sprite = tileSet[last].sprite;
                break;
        }

        CardTransitionSound.Stop();
        CardTransitionSound.Play();
    }

    public void UpdateInstant()
    {
        ActiveCard.sprite = tileSet[town.CurrentCard].sprite;
        if (playerDeck.TryGetLastDiscarded(out int dscrd))
            Discard.sprite = tileSet[dscrd].sprite;
        else
            Discard.sprite = emptySprite;
        if (playerDeck.TryPeekNext(out int nxtCard))
            DrawPile.sprite = tileSet[nxtCard].sprite;
        else
            DrawPile.sprite = emptySprite;
        DrawPileCount.text = playerDeck.DrawPile.Count.ToString();
        DiscardCount.text = playerDeck.Discard.Count.ToString();
    }

    public void PlayShuffle()
    {
        PlayAnim(DISCARD_TO_DRAW_ANIM);
        UpdateInstant();
    }
}
