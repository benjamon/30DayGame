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
			if (HP.Value <= 0)
				OnDeceased.Invoke();
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

		public EntityStats(int maxHP)
        {
			MaxHP = new Subvar<int>(maxHP);
			BaseArmor = new Subvar<int>(0);
			BonusDamage = new Subvar<int>(0);
		}

		public EntityStats(EntityStats stats)
        {
			MaxHP = new Subvar<int>(stats.MaxHP);
			BaseArmor = new Subvar<int>(stats.BaseArmor);
			BonusDamage = new Subvar<int>(stats.BonusDamage);
		}
	}
}
