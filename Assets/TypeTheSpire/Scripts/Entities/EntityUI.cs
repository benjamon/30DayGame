using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bentendo.TTS
{
	public class EntityUI : MonoBehaviour
	{
		public TMP_Text HP;
		public TMP_Text Stats;
		public Color ArmorColor;
		Subtext hp_subtext;
		Subtext stats_subtext;
		//public TMP_Text Armor;
		public void Setup(Entity e)
		{
			HP.text = HP.text.Replace("blue", ColorUtil.ColorToHexString(ArmorColor));
			hp_subtext = new Subtext(HP);
			hp_subtext.Subscribe(e.HP, ".hp");
			hp_subtext.Subscribe(e.Stats.MaxHP, ".max");
			hp_subtext.Subscribe(e.Armor, ".arm");
			stats_subtext = new Subtext(Stats);
			stats_subtext.Subscribe(e.Stats.BonusDamage, ".dmg");
			stats_subtext.Subscribe(e.Stats.BaseArmor, ".arm");
			stats_subtext.Subscribe(e.Stats.WordCost, ".wrd");
			stats_subtext.Subscribe(e.Stats.Speed, ".spd");
			//new Subtext(Armor, e.Armor);
			if (HP.transform.lossyScale.x <= 0f)
            {
				var scale = HP.transform.localScale;
				scale.x = -scale.x;
				HP.transform.localScale = scale;
            }
        }
	}

	[Serializable]
	public class Subtext
    {
		public TMP_Text text { get; private set; }
		List<Sub> subs = new List<Subtext.Sub>();
		string wrap = "1";

		public Subtext(TMP_Text text)
        {
			this.text = text;
			if (!string.IsNullOrWhiteSpace(text.text))
				wrap = text.text;
		}

		public void Subscribe(Subvar<int> val, string target)
        {
			subs.Add(new Sub(val, target, this));
			RebuildString();
        }

		void RebuildString()
        {
			var str = wrap;
			foreach (var sub in subs)
				str = sub.ApplyToString(str);
			text.text = str;
        }

		class Sub
        {
			public string Target;
			public string Replacement;
			Subtext owner;

			public Sub(Subvar<int> value, string target, Subtext owner)
            {
				this.Target = target;
				this.owner = owner;
				Replacement = value.ToString();
				value.onChanged.AddListener(UpdateText);
            }

			void UpdateText(int value)
            {
				Replacement = value.ToString();
				owner.RebuildString();
            }

			public string ApplyToString(string s)
            {
				return s.Replace(Target, Replacement);
            }
        }
    }
}
