using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new deck", menuName = "create new spell deck", order = 0)]
public class SpellDeck : ScriptableObject
{
    public WordSpell[] spells;

    public Deck<WordCard> GetStarterDeck()
    {
        var cards = new WordCard[spells.Length];
        for (int i = 0; i < cards.Length; i++)
            cards[i] = new WordCard(spells[i]);
        return new Deck<WordCard>(cards);
    }
}
