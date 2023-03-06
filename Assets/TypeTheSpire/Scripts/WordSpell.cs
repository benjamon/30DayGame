using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new word spell", menuName = "create new word spell", order = 0)]
public class WordSpell : ScriptableObject
{
    public Sprite icon;

    //temp  temp temp
    public int wordLength;
    public WordSpellAction action;
    public wSpellTarget spellTarget;

    public WordSpellAction[] actions;
    public WordSpellPayment[] payments;
}

[System.Serializable]
public class WordSpellAction
{
    public wSpellAction actionType;
    public wSpellTarget target;
    public float amount;

    public void InvokeOn(bEntity self, bEntity enemy)
    {
        switch (actionType)
        {
            case wSpellAction.block:
                self.AddBlock(amount);
                break;
            case wSpellAction.attack:
                enemy.DealDamage(amount);
                break;
            default:
                Debug.Log(actionType.ToString() + " for " + amount);
                break;
        }
    }
}

[System.Serializable]
public class WordSpellPayment {
    public wSpellPaymentMethod paymentType;
    public float cost;
}

public enum wSpellAction
{
    block,
    attack,
    heal,
    addTime,
    applyBuff,
    applyDebuff,
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

public enum wSpellPaymentMethod
{
    word,
    hp,
    time,
}