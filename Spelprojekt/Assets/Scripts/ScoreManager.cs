using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{

    float myTotalTime = 0;
    [SerializeField]
    TextMeshProUGUI myScoreTextMesh;
    
    
    void Start()
    {
        
    }
    void Update()
    {

    }
    float CountTime()
    {
        return myTotalTime = myTotalTime + Time.deltaTime;        
    }
    void UpdateTextMeshTotalTime ()
    {
        myScoreTextMesh.SetText(CountTime().ToString("2")); // To string definrar hur många decimaler jag vill ränka med, så i detta fallet blir det två decimaler
    }

}
