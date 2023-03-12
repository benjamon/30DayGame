using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCard
{
    public WordSpell spell { get; private set; }
    public int upgrades;

    public WordCard(WordSpell spell)
    {
        this.spell = spell;
    }
}
