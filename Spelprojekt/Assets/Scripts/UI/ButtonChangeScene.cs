using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonChangeScene : MonoBehaviour
{
    [SerializeField]
    AudioClip level1Song;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene(string aSceneName)
    {
        SceneManager.LoadScene(aSceneName);
        if (aSceneName == "Level_1_BOB")
        {
            AudioManager.ourPublicInstance.PlayMusicWithCrossFade(level1Song);
        }
        
    }
}
