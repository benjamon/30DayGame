using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypeTheSpire : MonoBehaviour
{
    public static TypeTheSpire Instance;

    public GameObject CardPrefab;
    public WordTyper kInput;
    WordProvider words;

    private void Awake()
    {
        Instance = this;
        if (WordProvider.Instance == null)
        {
            Debug.LogError("word provider not found");
            SceneManager.LoadScene(0);
            return;
        }
        words = WordProvider.Instance;
        var tw = CardPrefab.gameObject.GetComponent<TargetWord>();
        tw.Init(words.GetRandom(5));
        kInput.SetTarget(tw.ProcessCharacter);
    }

    public void TryFind(char c)
    {
        Debug.Log("trying to find " + c);
    }

    public void WordCompleted(TargetWord word)
    {
        Debug.Log("completed " + word.word);
        var tw = CardPrefab.gameObject.GetComponent<TargetWord>();
        tw.Init(words.GetRandom(5));
        kInput.SetTarget(tw.ProcessCharacter);
    }
}
