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
        Application.targetFrameRate = 60;
        StartCoroutine(LoadNext());
    }

    IEnumerator LoadNext()
    {
        yield return WordProvider.InitSingleton("google10000.txt", 12, 2);
        yield return new WaitForSeconds(.15f);
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}
