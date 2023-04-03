using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class AnimatedSprite : MonoBehaviour, IAnimRunnerTarget
    {
        public Sprite[] options;
        int randomOffset;
		SpriteRenderer spRender;

        void Start()
        {
            randomOffset = Random.Range(0, options.Length);
			spRender = GetComponent<SpriteRenderer>();
			AnimatedSpriteRunner.Instance.Sprites.Add(this);
        }

        public void UpdateSprite(int step)
		{
			spRender.sprite = options[(step + randomOffset) % options.Length];
		}

        void OnDestroy()
        {
            AnimatedSpriteRunner.Instance.Sprites.Remove(this);
        }
    }

	public interface IAnimRunnerTarget
	{
		void UpdateSprite(int step);
	}
}
