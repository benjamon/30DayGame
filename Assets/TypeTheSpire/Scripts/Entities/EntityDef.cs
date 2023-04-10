using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	[CreateAssetMenu(fileName = "new Entity Def", menuName = "new TTS Entity Def", order = 0)]
	public class EntityDef : ScriptableObject, IEntityProvider
	{
		public string Title;
		public int MaxHP;
		public Sprite EntitySprite;
		public CardDefCollection StarterDeck;
		public Sprite[] ScribbleFrames;

        public List<Card> GetCards() => StarterDeck.GetCards();
        public Entity GetEntity(BattleRunner runner) => new Entity(this, runner, false);
        public int GetHP() => MaxHP;
        public int GetMaxHP() => MaxHP;
		public EntityDef GetDef() => this;

		public void SetupSprite(SpriteRenderer sprite)
        {
			if (ScribbleFrames != null && ScribbleFrames.Length > 0)
            {
				sprite.gameObject.AddComponent<AnimatedSprite>().options = ScribbleFrames;
            } else
            {
				sprite.sprite = EntitySprite;
            }
        }
    }
}
