using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
    public class PlayerState : IEntityProvider
    {
        public EntityStats PlayerStats;
        List<Card> _cards;
        int _maxHP;
        int _hp;
        EntityDef _def;

        public PlayerState(EntityDef playerDef)
        {
            _maxHP = playerDef.GetMaxHP();
            _hp = playerDef.GetHP();
            _cards = playerDef.GetCards();
            _def = playerDef;
            PlayerStats = new EntityStats(playerDef.GetStats());
        }

        public void UpdateHealthToEntity(Entity e)
        {
            _hp = e.HP;
        }

        public EntityStats GetStats() => new EntityStats(PlayerStats);

        public List<Card> GetCards() => _cards;

        public EntityDef GetDef() => _def;

        public Entity GetEntity(BattleRunner runner) => new Entity(this, runner, true);

        public int GetHP() => _hp;

        public int GetMaxHP() => _maxHP;
    }
}
