using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bentendo
{
    [RequireComponent(typeof(TMP_Text))]
    public class AnimatedFont : MonoBehaviour, IAnimRunnerTarget
    {
        public TMP_FontAsset[] options;
        TMP_Text text;
        int randomOffset;

        void Start()
        {
            randomOffset = Random.Range(0, options.Length);
            text = GetComponent<TMP_Text>();
            AnimatedSpriteRunner.Instance.Sprites.Add(this);
        }

        public void UpdateSprite(int step)
        {
            text.font = options[(step + randomOffset) % options.Length];
        }
        void OnDestroy()
        {
            AnimatedSpriteRunner.Instance.Sprites.Remove(this);
        }
    }
}
