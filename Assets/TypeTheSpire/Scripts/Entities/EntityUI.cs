using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bentendo.TTS
{
	public class EntityUI : MonoBehaviour
	{
		public TMP_Text HP_Text;
		public TMP_Text MaxHP_Text;
		public void Setup(Entity e)
        {
			e.HP.action = UpdateHP;
			e.MaxHP.action = UpdateMaxHP;
			UpdateHP(e.HP);
			UpdateMaxHP(e.MaxHP);
        }

		public void UpdateHP(int hp)
        {
			HP_Text.text = hp.ToString();
        }

		public void UpdateMaxHP(int maxHP)
        {
			MaxHP_Text.text = maxHP.ToString();
        }
	}
}
