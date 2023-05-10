using UnityEngine;

namespace Bentendo.TTS
{
	[CreateAssetMenu(fileName = "Battle Event Def", menuName = "new TTS Battle Event Def", order = 0)]
	public class BattleEventDef : ScriptableObject, IEncounter
	{
		public EntityDef[] Enemies;
		public Sprite icon;

		public Sprite GetIcon() => icon;

		public Entity[] GetEntities(BattleRunner runner)
        {
			var res = new Entity[Enemies.Length];
			for (int i = 0; i < Enemies.Length; i++)
				res[i] = Enemies[i].GetEntity(runner);
			return res;
        }
	}
}
