using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LeverLoader : MonoBehaviour
{
    public Animator myTransition;
    
    [SerializeField]
    float myTransitionTime = 2f;
    int myLevelIndex = 0;

    // Singleton
    private static LeverLoader myInstance;
    public static LeverLoader ourInstance
    {
        get
        {
            return myInstance;
        }
    }

    private void Awake()
    {
        if (myInstance != null && myInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            myInstance = this;
        }
    }

    public void LoadNextLevel()
    {

        myLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (myLevelIndex > 2)
        {
            myLevelIndex = 0;
        }
        StartCoroutine(LoadLevel(myLevelIndex));

    }

    public void LoadLevelByName(string aSceneName)
    {
        StartCoroutine(LoadLevel(aSceneName));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        myTransition.SetTrigger("Start");
        yield return new WaitForSeconds(myTransitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevel (string aSceneName)
    {
        myTransition.SetTrigger("Start");
        yield return new WaitForSeconds(myTransitionTime);
        SceneManager.LoadScene(aSceneName);
    }
}
