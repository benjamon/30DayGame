using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bentendo.TTS
{
    public enum ActionTarget
    {
        SELF,
        ENEMY, // nearest
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
        MODIFY_STAT,
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
    public class Benums
    {
        public static string Stringify(StatID id)
        {
            var words = id.ToString().Split('_');    
            words = words
                .Select(word => char.ToUpper(word[0]) + word.Substring(1))
                .ToArray();
            return string.Join(' ', words);
        }
    }
}