using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    float myTotalTime = 0;
    bool myStartCounter = false;

    [Header("Score text")]
    [SerializeField]
    TextMeshProUGUI myScoreTextMesh;

    public float TotalTime
    {
        get
        {
            return myTotalTime;
        }
    }

    public bool StartCounter
    {
        set
        {
            myStartCounter = value;
        }
    }

    void Update()
    {
        if (myStartCounter)
        {
            UpdateTextMeshTotalTime();
        }
    }
    float CountTime()
    {
        return myTotalTime = myTotalTime + Time.deltaTime;
    }
    void UpdateTextMeshTotalTime()
    {
        myScoreTextMesh.SetText(CountTime().ToString("0.00")); // To string definrar hur många decimaler jag vill ränka med, så i detta fallet blir det två decimaler
    }
    public void ResetTimer()
    {
         myTotalTime = 0;
    }

}
