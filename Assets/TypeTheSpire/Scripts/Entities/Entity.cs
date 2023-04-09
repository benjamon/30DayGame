using System;
using UnityEngine.Events;

namespace Bentendo.TTS
{
    public class Entity
	{
		public IEntityProvider Source { get; private set; }
		public Subvar<int> MaxHP;
		public Subvar<int> HP;
		public Deck<Card> Deck { get; private set; }
		public EntityBody Body { get; private set; }
		public UnityEvent OnDeceased = new UnityEvent();
		public bool isLeft { get; private set; }
		BattleRunner runner;

		public Entity(IEntityProvider provider, BattleRunner runner, bool isLeft)
        {
			this.Source = provider;
			this.runner = runner;
			this.isLeft = isLeft;
			MaxHP = new Subvar<int>(provider.GetMaxHP());
			HP = new Subvar<int>(provider.GetHP());
			Deck = new Deck<Card>(provider.GetCards());
        }

		public void SetBody(EntityBody body)
        {
			this.Body = body;
			body.Setup(this);
        }

		public void ApplyDamage(Damage dmg)
        {
			HP.Value -= dmg.amount;
			if (HP.Value < 0)
				OnDeceased.Invoke();
        }

		public void PlayCard(CastInfo info)
        {
			info.source = this;
			runner.PlayCardImminent(info);
        }

        internal void AddHealth(int amount)
        {
			HP.Value = Math.Min(HP.Value + amount, MaxHP.Value);
        }
    }

	public class Damage
	{
		public Entity source;
		public int amount;

		public Damage(Entity e, int amt)
        {
			source = e;
			amount = amt;
        }
    }
}
