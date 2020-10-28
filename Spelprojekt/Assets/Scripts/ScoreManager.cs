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
  
    string myAmountOfDecimals = "0.00";
    int id;
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
    void Start()
    {
        id = Animator.StringToHash(myAmountOfDecimals);
        
    }
    void Update()
    {
        if (myStartCounter)
        {
            UpdateTextMeshTotalTime();
        }
    }
    int CountTime()
    {
        return Mathf.FloorToInt(myTotalTime = myTotalTime + Time.deltaTime);
    }
    void UpdateTextMeshTotalTime()
    {
        
        if (
            myScoreTextMesh != null)
        {
           myScoreTextMesh.SetText(CountTime().ToString()); // To string definrar hur många decimaler jag vill ränka med, så i detta fallet blir det två decimaler

        }
    }
    public void ResetTimer()
    {
         myTotalTime = 0;
    }

}
