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

		public Entity(IEntityProvider provider)
        {
			this.Source = provider;
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
			HP.crnt -= dmg.amount;
			if (HP.crnt < 0)
				OnDeceased.Invoke();
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
