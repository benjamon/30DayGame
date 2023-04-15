using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bentendo.TTS
{
	public class CardPileUI : MonoBehaviour
	{
		public SpriteRenderer CoverImage;
		public TMP_Text text;

		public void UpdatePile(Sprite cover, int count)
		{
			text.text = count.ToString();
		}
    }
}
