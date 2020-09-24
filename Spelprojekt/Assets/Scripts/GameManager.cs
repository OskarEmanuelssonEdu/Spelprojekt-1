using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Countdown Before Game Begins")]
    [SerializeField]
    float myCountDownTime = 3;
    float myCountDownTimer = 0;
   
    [Header("Level Compelet Screen Settings")]
    [SerializeField]
    GameObject myLevelCompleteScreen;
    [SerializeField]
    GameObject myGameOverScreen;

    [SerializeField]
    TextMeshProUGUI myTotalTimeText;

    [Header("Reference Settings")]
    [SerializeField]
    ScoreManager myScoreManager;
    [SerializeField]
    Player myPlayer;
   

    void Update()
    {
        CountDownToStart();
       
        if (myPlayer.myCurrentHealth <= 0)
        {
            GameOver();
        }
    }
    void CountDownToStart()
    {
        if (myCountDownTimer >= myCountDownTime)
        {
            // Game has started and player can move :D
            StartGame();
        }
        else
        {
            myCountDownTimer += Time.deltaTime;
        }
    }
    void LevelComplete()
    {
        myLevelCompleteScreen.SetActive(true);
        myTotalTimeText.text = myScoreManager.TotalTime.ToString("0.00");
    }
    void GameOver()
    {
        myGameOverScreen.SetActive(true);
        myTotalTimeText.text = myScoreManager.TotalTime.ToString("0.00");

    }
    public Vector3 PlayerPosition()
    {
        return myPlayer.transform.position;
    }
    void StartGame()
    {
        if (myScoreManager != null)
        {
            myScoreManager.StartCounter = true;
        }

    }
}
