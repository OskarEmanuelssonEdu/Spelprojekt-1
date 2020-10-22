using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerTest : MonoBehaviour
{
    [SerializeField]
    private AudioClip myButtonClickSFX;
    [SerializeField]
    private AudioClip myMusic1;
    [SerializeField]
    private AudioClip myMusic2;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.ourPublicInstance.PlaySFX1(myButtonClickSFX, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AudioManager.ourPublicInstance.PlayMusic(myMusic1);
            AudioManager.ourPublicInstance.SetMusicVolume(0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AudioManager.ourPublicInstance.PlayMusic(myMusic2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AudioManager.ourPublicInstance.PlayMusicWithFade(myMusic1, 5f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AudioManager.ourPublicInstance.PlayMusicWithFade(myMusic2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AudioManager.ourPublicInstance.PlayMusicWithCrossFade(myMusic1, 3.0f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            AudioManager.ourPublicInstance.PlayMusicWithCrossFade(myMusic2, 3.0f);
        }


    }

}
