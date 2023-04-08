using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bentendo.TTS
{
	public class EntityUI : MonoBehaviour
	{
		public TMP_Text HP_Text;
		int lastHP;
		int lastMaxHP;
		public void Setup(Entity e)
        {
			e.HP.onChanged.AddListener(UpdateHP);
			e.MaxHP.onChanged.AddListener(UpdateMaxHP);
			UpdateHP(e.HP);
			UpdateMaxHP(e.MaxHP);
        }

		public void UpdateHP(int hp)
        {
			lastHP = hp;
			UpdateHealthText();
		}

		public void UpdateMaxHP(int maxHP)
        {
			lastMaxHP = maxHP;
			UpdateHealthText();
        }

		public void UpdateHealthText()
        {
			HP_Text.text = $"hp {lastHP} max {lastMaxHP}";
        }
	}
}
