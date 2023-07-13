using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bentendo.TTS
{
	public class EmbodiedCard : MonoBehaviour
	{
		public SpriteRenderer Icon;
		public TMP_Text Title;
		public WordFX FX;

		WordTarget target;
		Action<CastInfo> castAction;
		CastInfo info;
		Card card;

		public void Setup(Card card, Action<CastInfo> castAction)
        {
			this.card = card;
            this.castAction = castAction;
			Title.text = $"{card.def.title} {card.def.actions[0].amount}";
			Icon.sprite = card.def.Icon;
        }

		public void SetWord(string word)
        {
			target = new WordTarget(word);
			info = new CastInfo
			{
				card = card,
				word = word,
			};
            target.onCompleted += OnComplete;
			FX.SubscribeWordTarget(target);
        }

		public void FocusCard(KInput input, char c)
		{
			info.timeToType = Time.time;
			target.OnTypo = MakeTypo;
			input.SetProcessAction(target.ProcessPress);
			target.ProcessPress(c);
		}

		public void MakeTypo()
        {
			info.typos++;
			FX.ProcessTypo();
		}

		public void OnComplete()
		{
			info.timeToType = Time.time - info.timeToType;
			castAction.Invoke(info);
        }
	}
}
