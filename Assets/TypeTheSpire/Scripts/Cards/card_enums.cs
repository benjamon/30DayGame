using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
    public enum ActionTarget
    {
        SELF,
        NEAREST,
        RANDOM,
        ALL,
        ALL_ENEMIES,
        ALL_ALLIED,
    }

    public enum ActionType
    {
        PLAY_CARD,
        STRIKE,
        BLOCK,
        HEAL,
        APPLY_STATUS,
        DRAW,
        DISCARD,
    }

    public enum StatusEffect
    {
        POISONED,
        VULERNABLE,
        WEAK,
        CONFUSED,
        CHARMED,
        CRACKED,
    }

    public enum WordSource
    {
        DEFAULT,
        FRUITS,
        ANIMALS,
        SYMBOLS,
        PRONOUNS,
        SCIENCE,
    }
}
