namespace Bentendo.TTS
{
    public class EntityStats
	{
		public Subvar<int> MaxHP;
		public Subvar<int> BaseArmor;
		public Subvar<int> BonusDamage;
		public Subvar<int> Speed;
		public Subvar<int> Initiative;
		public Subvar<int> Slow;
		public Subvar<int> WordLengthModifier;

		public EntityStats(int maxHP)
        {
			MaxHP = new Subvar<int>(maxHP);
			BaseArmor = new Subvar<int>(0);
			BonusDamage = new Subvar<int>(0);
			Speed = new Subvar<int>(0);
			Slow = new Subvar<int>(0);
			Initiative = new Subvar<int>(0);
			WordLengthModifier = new Subvar<int>(0);
		}

		public EntityStats(EntityStats stats)
        {
			MaxHP = new Subvar<int>(stats.MaxHP);
			BaseArmor = new Subvar<int>(stats.BaseArmor);
			BonusDamage = new Subvar<int>(stats.BonusDamage);
			Speed = new Subvar<int>(stats.Speed);
			Slow = new Subvar<int>(stats.Slow);
			WordLengthModifier = new Subvar<int>(stats.WordLengthModifier);
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
                case StatID.WORD_LENGTH:
					return WordLengthModifier;
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
		WORD_LENGTH,
	}

}
