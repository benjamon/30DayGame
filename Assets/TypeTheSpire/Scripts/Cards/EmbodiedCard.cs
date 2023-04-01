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
		public WordFX Word;

		WordTarget target;
		Action castAction;
		
		public void Setup(Card card, Action castAction)
        {
            this.castAction = castAction;
			Title.text = card.def.title;
			Icon.sprite = card.def.Icon;
        }

		public void ShowWord(string word)
        {
			target = new WordTarget(word);
            target.onCompleted += OnComplete;
			Word.SubscribeWordTarget(target);
        }

		public void FocusCard(KInput input, char c)
		{
			target.ProcessPress(c);
			input.SetProcessAction(target.ProcessPress);
		}

		public void OnComplete()
		{
			castAction.Invoke();
        }
	}
}
