using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public int nextScene;

    private void Start()
    {
        StartCoroutine(LoadNext());
    }

    IEnumerator LoadNext()
    {
        yield return WordProvider.InitSingleton("google10000.txt", 12, 2);
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}
