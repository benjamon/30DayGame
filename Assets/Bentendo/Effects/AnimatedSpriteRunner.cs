using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo
{
	public class AnimatedSpriteRunner : MonoBehaviour
	{
        static AnimatedSpriteRunner instance;
        public static AnimatedSpriteRunner Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject("AnimatedSpriteRunner");
                    instance = go.AddComponent<AnimatedSpriteRunner>();
                }
                return instance;
            }
        }

        [NonSerialized]
        public List<IAnimRunnerTarget> Sprites = new List<IAnimRunnerTarget>();
        public static float FPS = 4f;
        int lastStep = 0;
        void Update()
        {
            int step = Mathf.FloorToInt(Time.time * FPS);
            if (lastStep != step)
            {
                lastStep = step;
                for (int i = 0; i < Sprites.Count; i++)
                {
                    var sprite = Sprites[i];
                    sprite.UpdateSprite(step);
                }
            }
        }
    }
}
