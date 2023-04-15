using System;
using UnityEngine.Events;

namespace Bentendo.TTS
{
    public class Entity
	{
		public IEntityProvider Source { get; private set; }

		public Subvar<int> HP;

		public Subvar<int> Armor = new Subvar<int>(0);

		public EntityStats Stats;

		public Deck<Card> Deck { get; private set; }
		public EntityBody Body { get; private set; }
		public UnityEvent OnDeceased = new UnityEvent();
		public UnityEvent OnBlocked = new UnityEvent();
		public bool isLeft { get; private set; }
		public bool isAlive { get; private set; }
		BattleRunner runner;

		public Entity(IEntityProvider provider, BattleRunner runner, bool isLeft)
        {
			this.Source = provider;
			this.runner = runner;
			this.isLeft = isLeft;
			Stats = new EntityStats(provider.GetStats());
			HP = new Subvar<int>(provider.GetHP());
			Deck = new Deck<Card>(provider.GetCards());
			Armor.Value = Stats.BaseArmor;
			isAlive = true;
        }

		public void SetBody(EntityBody body)
        {
			this.Body = body;
			body.Setup(this);
        }

		public void ApplyArmor(int amount)
        {
			Armor.Value += amount;
        }

		public void ApplyDamage(Damage dmg)
        {
			int amount = dmg.GetAmount(this);
			if (Armor > amount)
            {
				Armor.Value = Stats.BaseArmor;
				OnBlocked.Invoke();
				return;
            }

			amount -= Armor.Value;
			Armor.Value = Stats.BaseArmor;

			HP.Value -= amount;
			if (isAlive && HP.Value <= 0)
			{
				isAlive = false;
				OnDeceased.Invoke();
			}
        }

		public void PlayCard(CastInfo info)
        {
			info.source = this;
			runner.PlayCardImminent(info);
        }

        internal void AddHealth(int amount)
        {
			HP.Value = Math.Min(HP.Value + amount, Stats.MaxHP.Value);
        }
    }

	public class Damage
	{
		public Entity source;
		int amount;

		public Damage(Entity e, int amt)
        {
			source = e;
			amount = amt;
        }

		public int GetAmount(Entity target)
        {
			if (amount == 0)
				return 0;
			return (source != target) ? amount + source.Stats.BonusDamage : amount;
        }
    }

	public class EntityStats
	{
		public Subvar<int> MaxHP;
		public Subvar<int> BaseArmor;
		public Subvar<int> BonusDamage;
		public Subvar<int> Speed;
		public Subvar<int> Slow;
		public Subvar<int> WordCost;

		public EntityStats(int maxHP)
        {
			MaxHP = new Subvar<int>(maxHP);
			BaseArmor = new Subvar<int>(0);
			BonusDamage = new Subvar<int>(0);
			Speed = new Subvar<int>(0);
			Slow = new Subvar<int>(0);
			WordCost = new Subvar<int>(0);
		}

		public EntityStats(EntityStats stats)
        {
			MaxHP = new Subvar<int>(stats.MaxHP);
			BaseArmor = new Subvar<int>(stats.BaseArmor);
			BonusDamage = new Subvar<int>(stats.BonusDamage);
			Speed = new Subvar<int>(stats.Speed);
			Slow = new Subvar<int>(stats.Slow);
			WordCost = new Subvar<int>(stats.WordCost);
		}

		public Subvar<int> GetStat(StatID id)
        {
            switch (id)
            {
                case StatID.MAX_HP:
                    return MaxHP;
                case StatID.BASE_ARMOR:
                    return BaseArmor;
                case StatID.SPEED:
					return Speed;
                case StatID.SLOW:
					return Slow;
                case StatID.WORD_COST:
					return WordCost;
                case StatID.BONUS_DAMAGE:
                default:
                    return BonusDamage;
            }
        }
	}

	public enum StatID
	{
		MAX_HP,
		BASE_ARMOR,
		BONUS_DAMAGE,
		SPEED,
		SLOW,
		WORD_COST,
	}

}
