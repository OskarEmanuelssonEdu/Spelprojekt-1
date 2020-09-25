using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeverLoader : MonoBehaviour
{
    public Animator myTransition;
    
    float myTransitionTime = 2f;
    int myLevelIndex = 0;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(2))
        {
            LoadNextLevel();
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

    IEnumerator LoadLevel(int levelIndex)
    {
        myTransition.SetTrigger("Start");
        yield return new WaitForSeconds(myTransitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
