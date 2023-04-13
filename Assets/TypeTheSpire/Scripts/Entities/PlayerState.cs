using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
    public class PlayerState : IEntityProvider
    {
        public EntityStats PlayerStats;
        public Subvar<int> HP;
        List<Card> _cards;
        EntityDef _def;

        public PlayerState(EntityDef playerDef)
        {
            HP = new Subvar<int>(playerDef.GetHP());
            _cards = playerDef.GetCards();
            _def = playerDef;
            PlayerStats = new EntityStats(playerDef.GetStats());
        }

        public void UpdateHealthToEntity(Entity e)
        {
            HP = e.HP;
        }

        public EntityStats GetStats() => new EntityStats(PlayerStats);

        public List<Card> GetCards() => _cards;

        public EntityDef GetDef() => _def;

        public Entity GetEntity(BattleRunner runner) => new Entity(this, runner, true);

        public int GetHP() => HP;

        public int GetMaxHP() => PlayerStats.MaxHP;
    }
}
