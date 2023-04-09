using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
    public class PlayerState : IEntityProvider
    {
        List<Card> _cards;
        int _maxHP;
        int _hp;
        Sprite _sprite;

        public PlayerState(IEntityProvider playerDef)
        {
            _maxHP = playerDef.GetMaxHP();
            _hp = playerDef.GetHP();
            _cards = playerDef.GetCards();
            _sprite = playerDef.GetSprite();
        }

        public void UpdateHealthToEntity(Entity e)
        {
            _maxHP = e.MaxHP;
            _hp = e.HP;
        }

        public List<Card> GetCards()
        {
            return _cards;
        }

        public Entity GetEntity(BattleRunner runner) => new Entity(this, runner, true);

        public int GetHP() => _hp;

        public int GetMaxHP() => _maxHP;

        public Sprite GetSprite() => _sprite;
    }
}
