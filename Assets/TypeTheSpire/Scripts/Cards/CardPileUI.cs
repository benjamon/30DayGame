using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class CardPileUI : MonoBehaviour
	{
		public SpriteRenderer CoverImage;
		public float CardHeight = 0.1f;

		public void Setup(Sprite cover, int count)
		{
			CoverImage.sprite = cover;
			CoverImage.transform.position = Vector3.up * count * CardHeight;
		}
	}
}
