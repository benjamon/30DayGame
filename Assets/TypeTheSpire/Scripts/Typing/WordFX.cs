using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bentendo.TTS
{
	public class WordFX : MonoBehaviour
	{
		public TMP_Text WordText;
		public GameObject effect;
        public AudioSource honker;
        public Color doneColor;
        public Color lastColor;
        public Color notDoneColor;
        public Color inactiveColor;
        WordTarget target;
        string word;
        KInput kin;
        WordProvider words;

        private void Start()
        {
            if (WordProvider.Instance == null)
            {
                Debug.LogError("word provider not found");
                SceneManager.LoadScene(0);
                return;
            }
            words = WordProvider.Instance;

            kin = GameObject.FindObjectOfType<KInput>();
            ResetTarget();
        }

        public void ResetTarget()
        {
            word = words.GetRandom(8);
            target = new WordTarget(word);
            target.onCharTyped = UpdateText;
            target.onCompleteed = ResetTarget;
            target.OnTypo = Honk;
            WordText.text = WrapColor(word, inactiveColor);
            kin.SetAction(target.ProcessPress, Honk);
        }

        public void UpdateText()
        {
            var n = target.completion;
            string str;
            str = WrapColor(word.Substring(0, n), doneColor);
            var go = GameObject.Instantiate(effect, null);
            var charTMP = go.GetComponent<TMP_Text>();
            go.transform.localScale = WordText.transform.localScale;
            if (charTMP != null)
            {
                charTMP.text = word[n - 1].ToString();
                charTMP.fontSize = WordText.fontSize;
                AlignWords(WordText, n - 1, charTMP, 0);
            }
            GameObject.Destroy(go, .125f);
            if (n != word.Length)
                str += WrapColor(word[n].ToString(), lastColor);
            if (n <= word.Length - 2)
                str += WrapColor(word.Substring(n + 1, word.Length - n - 1), notDoneColor);
            WordText.text = str;
        }

        public string WrapColor(string s, Color c)
        {
            return $"<color={ColorToHexString(c)}>{s}</color>";
        }

        public void Honk()
        {
            honker.Play();
        }
        public static string ColorToHexString(Color color)
        {
            int r = (int)(color.r * 255);
            int g = (int)(color.g * 255);
            int b = (int)(color.b * 255);
            int a = (int)(color.a * 255);

            string hex = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
            return hex;
        }

        public Vector2 GetLetterCenter(int x)
        {
            var ci = WordText.textInfo.characterInfo[x];
            return (ci.vertex_BL.position + ci.vertex_BR.position +
                   ci.vertex_TL.position + ci.vertex_TR.position) * .25f;
        }

        public Vector2 GetLetterLeft(int x)
        {
            var ci = WordText.textInfo.characterInfo[x];
            return new Vector2(ci.vertex_BL.position.x, 0f);
        }

        public void AlignWords(TMP_Text a, int an, TMP_Text b, int bn)
        {
            if (an == 0)
            {
                b.transform.position = a.transform.TransformPoint(a.textBounds.min.x * Vector3.right);
                return;
            }
            var aci = a.textInfo.characterInfo[an-1];
            var bci = b.textInfo.characterInfo[bn];
            Vector2 worldDiff = new Vector2((b.transform.TransformPoint(Vector3.right * bci.xAdvance) - a.transform.TransformPoint(Vector3.right * aci.xAdvance)).x, 0f);
            b.transform.position -= new Vector3(worldDiff.x, 0f, 0f);
        }
    }
}
