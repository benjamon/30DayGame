namespace Bentendo.TTS
{
    public class Entity
	{
		public IEntityProvider Source { get; private set; }
		public Actvar<int> MaxHP;
		public Actvar<int> HP;
		public Deck<Card> Deck { get; private set; }
		public EntityBody Body { get; private set; }
		//public List<StatusEffect> { etc }

		public Entity(IEntityProvider provider)
        {
			this.Source = provider;
			MaxHP = new Actvar<int>(provider.GetMaxHP());
			HP = new Actvar<int>(provider.GetHP());
			Deck = new Deck<Card>(provider.GetCards());
        }

		public void SetBody(EntityBody body)
        {
			this.Body = body;
			body.Setup(this);
        }
	}
}
