using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new word spell", menuName = "create new word spell", order = 0)]
public class WordSpell : ScriptableObject
{
    public Sprite icon;

    //expand into an array and maybe have different word types
    public int wordLength;
    public wSpellTarget spellTarget;
    public WordSpellAction action;
}

[System.Serializable]
public class WordSpellAction
{
    public wSpellAction actionType;
    public wSpellTarget target;
    public wStatusEffect status;
    public int amount;

    public void InvokeOn(bEntity self, bEntity enemy)
    {
        var e = (target != wSpellTarget.self) ? enemy : self;
        switch (actionType)
        {
            case wSpellAction.block:
                e.AddBlock(amount);
                break;
            case wSpellAction.attack:
                if (self.has(wStatusEffect.weak))
                    if (amount > 0)
                        amount = System.Math.Max(1, amount / 2);
                e.DealDamage(amount);
                break;
            case wSpellAction.applyStatus:
                e.ApplyStatus(status, amount);
                break;
            default:
                Debug.Log(actionType.ToString() + " for " + amount);
                break;
        }
    }
}

[SerializeField]
public class WordSpellStatusEffect
{
    public wStatusEffect statusType;
    public int amount;
}

public enum wSpellAction
{
    block,
    attack,
    heal,
    addTime,
    applyStatus,
}

public enum wSpellTarget
{
   inherit,
   nearest,
   furthest,
   allEnemies,
   self,
   allFriendly,
   all,
}

public enum wStatusEffect
{
    vulernable,
    weak,
    rage,
    protect,
    slow,
    poison,
    doom,
}