using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change_Scene : MonoBehaviour
{
    private float maxLoad = 0.5f;
    public void nextScene()
    {
        StartCoroutine(LoadScene(maxLoad));
        
    }

    public void previousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }

    public void rollCredits()
    {
        SceneManager.LoadScene(2);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator LoadScene(float loadingTime)
    {
        while(loadingTime > 0)
        {
            loadingTime -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        loadingTime = maxLoad;
    }

}
