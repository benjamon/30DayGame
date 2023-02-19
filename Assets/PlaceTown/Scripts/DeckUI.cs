using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour
{
    public TMP_Text DrawPileCount;
    public TMP_Text DiscardCount;
    public Image ActiveCard;
    public Image NextCard;
    public Image Discard;
    public AudioSource CardTransitionSound;

    PlaceTown town;
    Deck<int> playerDeck;
    Bmap.TileDef[] tileSet;
    Sprite emptySprite;

    public void Init(Deck<int> deck_, Bmap.TileDef[] tileSet_, PlaceTown town_)
    {
        tileSet = tileSet_;
        emptySprite = Discard.sprite;
        town = town_;
        playerDeck = deck_;
        UpdateInstant();
    }

    public IEnumerator UpdateUI()
    {

        CardTransitionSound.Stop();
        CardTransitionSound.Play();

        ActiveCard.sprite = emptySprite;

        if (playerDeck.TryGetLastDiscarded(out int dscrd))
            Discard.sprite = tileSet[dscrd].sprite;
        else
            Discard.sprite = emptySprite;

        DiscardCount.text = playerDeck.Discard.Count.ToString();

        yield return new WaitForSeconds(.15f);

        CardTransitionSound.Stop();
        CardTransitionSound.Play();

        ActiveCard.sprite = tileSet[town.CurrentCard].sprite;

        if (playerDeck.TryPeekNext(out int nxtCard))
            NextCard.sprite = tileSet[nxtCard].sprite;
        else
            NextCard.sprite = emptySprite;

        DrawPileCount.text = playerDeck.DrawPile.Count.ToString();
    }

    public void UpdateInstant()
    {
        ActiveCard.sprite = tileSet[town.CurrentCard].sprite;
        if (playerDeck.TryGetLastDiscarded(out int dscrd))
            Discard.sprite = tileSet[dscrd].sprite;
        else
            Discard.sprite = emptySprite;
        if (playerDeck.TryPeekNext(out int nxtCard))
            NextCard.sprite = tileSet[nxtCard].sprite;
        else
            NextCard.sprite = emptySprite;
        DrawPileCount.text = playerDeck.DrawPile.Count.ToString();
        DiscardCount.text = playerDeck.Discard.Count.ToString();
    }
    public void UpdateWithSound()
    {
        UpdateInstant();
        CardTransitionSound.Stop();
        CardTransitionSound.Play();
    }
}
