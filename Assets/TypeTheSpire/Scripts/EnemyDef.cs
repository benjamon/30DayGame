using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new enemy", menuName = "create new enemy def", order = 0)]
public class EnemyDef : ScriptableObject
{
    public SpellDeck deck;
    public Sprite sprite;
    public int hp;

    public void Setup(EntityActor actor)
    {
        actor.charImage.sprite = sprite;
        actor.SetDeck(deck.GetStarterDeck());
    }
}
