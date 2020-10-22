using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonQuit : MonoBehaviour
{
    public void QuitApplication()
    {
        Debug.Log("The Game should quit now...");
        Application.Quit();
    }
}
