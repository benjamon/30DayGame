using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bentendo.TTS
{
	public class EntityUI : MonoBehaviour
	{
		public TMP_Text HP_Text;
		public Color AtMax, BelowMax;
		int lastHP;
		int lastMaxHP;
		public void Setup(Entity e)
        {
			e.HP.onChanged.AddListener(UpdateHP);
			e.MaxHP.onChanged.AddListener(UpdateMaxHP);
			UpdateHP(e.HP);
			UpdateMaxHP(e.MaxHP);
			if (HP_Text.transform.lossyScale.x <= 0f)
            {
				var scale = HP_Text.transform.localScale;
				scale.x = -scale.x;
				HP_Text.transform.localScale = scale;
            }
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
			HP_Text.color = (lastHP != lastMaxHP) ? BelowMax : AtMax;
			HP_Text.text = lastHP.ToString();
        }
	}
}
