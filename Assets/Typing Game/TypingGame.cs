using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TypingGame : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public TMP_Text playerText;
    public Image TestKeyboardAPIPanel;
    TouchScreenKeyboard m_Keyboard;

    Dictionary<string, EnemyDisplay> active = new Dictionary<string, EnemyDisplay>();

    WordProvider words;

    int min = 3;
    int max = 8;
    int len = 4;

    public void Start()
    {
        Application.targetFrameRate = 45;
        words = new WordProvider("dictionary.txt", max);
        Sequencer.main.WaitUntil(words.IsSetup);
        Sequencer.main.Enqueue(LogWords);
    }

    string inpt = "";
    float backPressed = 0f;

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (m_Keyboard == null || !m_Keyboard.active || m_Keyboard.status != TouchScreenKeyboard.Status.Visible)
            {
                inpt = "";
                TouchScreenKeyboard.hideInput = true;
                m_Keyboard = TouchScreenKeyboard.Open(inpt, TouchScreenKeyboardType.Default, false, false, true);
                TouchScreenKeyboard.Android.consumesOutsideTouches = false;
            }
            float height = GetKeyboardHeight(false);
            float scaled = height / Screen.currentResolution.height;
            TestKeyboardAPIPanel.rectTransform.anchoredPosition = new Vector2(0f, scaled * 100);
            Debug.Log("KEYBOARD HEIGHT: " + height);
            Debug.Log("KEYBOARD HEIGHT SCALED: " + scaled);
            len = Mathf.FloorToInt(Mathf.Min(max, min + Time.time / 15f));
            inpt = m_Keyboard.text;
            playerText.text = inpt;
            inpt = inpt.ToLower();
            if (active.ContainsKey(inpt))
            {
                active[inpt].Kill();
                active.Remove(inpt);
                m_Keyboard.text = "";
                inpt = "";
            }
        } else
        {
            len = Mathf.FloorToInt(Mathf.Min(max, min + Time.time / 10f));
            if (Input.anyKeyDown)
            {
                for (int i = 97; i < 97 + 26; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                        inpt = "";
                    if (Input.GetKeyDown(KeyCode.Backspace) && backPressed - Time.time < -.05f)
                    {
                        inpt = (inpt.Length > len) ? inpt.Substring(len, inpt.Length - 1) : "";
                        backPressed = Time.time;
                    }
                    if (Input.GetKeyDown((KeyCode)i))
                    {
                        inpt = inpt + ((KeyCode)i).ToString().ToLower();
                        if (active.ContainsKey(inpt))
                        {
                            active[inpt].Kill();
                            active.Remove(inpt);
                            inpt = "";
                        }
                    }
                }
                playerText.text = inpt;
            }
        }
    }

    IEnumerator LogWords()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.15f);
            StartCoroutine(SpawnEnemy());
            yield return new WaitForSeconds(3f);
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        int l = Random.Range(min, len+1);
        string w = words.GetRandom(l).ToLower();
        while (active.ContainsKey(w)) w = words.GetRandom(l).ToLower();
        var go = GameObject.Instantiate(EnemyPrefab);
        var ed = go.GetComponent<EnemyDisplay>();
        active.Add(w, ed);
        go.transform.position = new Vector3(Random.Range(-3f, 3f), Random.Range(4f, 6f), 0f);
        ed.SetWord(w);
        yield return new WaitForSeconds(4f);
        ed.Destroy();
        if (active.ContainsKey(w))
            active.Remove(w);
    }

    public static int GetKeyboardHeight(bool includeInput)
    {
#if UNITY_ANDROID
        using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            var unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
            var view = unityPlayer.Call<AndroidJavaObject>("getView");
            var dialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");

            if (view == null || dialog == null)
                return 0;

            var decorHeight = 0;

            if (includeInput)
            {
                var decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");

                if (decorView != null)
                    decorHeight = decorView.Call<int>("getHeight");
            }

            using (var rect = new AndroidJavaObject("android.graphics.Rect"))
            {
                view.Call("getWindowVisibleDisplayFrame", rect);
                return Display.main.systemHeight - rect.Call<int>("height") + decorHeight;
            }
        }
#else
        var height = Mathf.RoundToInt(TouchScreenKeyboard.area.height);
        return height >= Display.main.systemHeight ? 0 : height;
#endif
    }
}
