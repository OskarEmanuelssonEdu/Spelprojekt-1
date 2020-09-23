using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    float myCountDownTime = 3;
    float myCountDownTimer = 0;

    

    void Update()
    {
        CountDownToStart();
        
    }
    void CountDownToStart()
    {
        if (myCountDownTimer>= myCountDownTime)
        {
            // Game has started and player can move :D

        }
        else
        {
            myCountDownTimer += Time.deltaTime;
        }
    }
}
