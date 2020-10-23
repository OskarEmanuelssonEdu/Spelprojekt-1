using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliderValue : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI myTextMeshProObject;

    public void SetMasterVolume(float aVolume)
    {
        myTextMeshProObject.text = "Master Volume: " + ((int)(aVolume * 100)).ToString() + "%";
    }
}
