﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [Header("Countdown Before Game Begins")]
    [SerializeField]
    float myCountDownTime = 3;
    float myCountDownTimer = 0;
    [SerializeField]
    GameObject[] myCountDownText;
    [SerializeField]
    GameObject myCountDownTextContainer;

    [Header("Level Compelet Screen Settings")]
    [SerializeField]
    GameObject myLevelCompleteScreen;
    [SerializeField]
    GameObject myGameOverScreen;

    [SerializeField]
    TextMeshProUGUI myTotalTimeText;


    [SerializeField]
    ScoreManager myScoreManager;
    [SerializeField]
    PlayerMovement myPlayerMovement;
    [SerializeField]
    GrappleHookBoohyah myGrappleHook;
    [SerializeField]
    Player myPlayer;
    Vector3 startPos;
    [SerializeField]
    NewCameraMovement myCamera;
    [SerializeField]
    LevelManager myLevelManager;


    [Header("SOUND")]
    [SerializeField]
    private AudioClip myDeathSoundClip;
    void OnValidate()
    {
        myPlayer = FindObjectOfType<Player>();
        myPlayerMovement = FindObjectOfType<PlayerMovement>();
        myGrappleHook = FindObjectOfType<GrappleHookBoohyah>();
        myScoreManager = FindObjectOfType<ScoreManager>();
        myCamera = FindObjectOfType<NewCameraMovement>();
        myLevelManager = FindObjectOfType<LevelManager>();

    }
    void Start()
    {
        myGrappleHook.enabled = true;
        myPlayerMovement.enabled = true;
        startPos = myPlayer.transform.position;
    }
    void Update()
    {
        CountDownToStart();
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame(true);
        }
        if (myPlayer.myCurrentHealth <= 0)
        {
            ResetGame(false);
        }
    }
    void CountDownToStart()
    {
        if (myCountDownTimer >= myCountDownTime)
        {
            // Game has started and player can move :D
            StartGame();
            if (myCountDownText != null)
            {

                myCountDownText[1].gameObject.SetActive(false);
                myCountDownText[0].gameObject.SetActive(true);


            }

        }
        else
        {
            if (Mathf.CeilToInt(myCountDownTime - myCountDownTimer) - 1 < myCountDownText.Length)
            {

                myCountDownText[Mathf.CeilToInt(myCountDownTime - myCountDownTimer) - 1].gameObject.SetActive(true);



                if ((myCountDownTime - myCountDownTimer) % 1 <= 0.5f)
                {

                    myCountDownText[Mathf.CeilToInt(myCountDownTime - myCountDownTimer) - 1].transform.position = Vector3.Lerp(myCountDownText[Mathf.CeilToInt(myCountDownTime - myCountDownTimer) - 1].transform.position, Vector3.zero + myCountDownTextContainer.transform.position, 0.5f);


                }
                else
                {
                    if (Mathf.CeilToInt(myCountDownTime - myCountDownTimer) < myCountDownText.Length)
                    {

                        myCountDownText[Mathf.CeilToInt(myCountDownTime - myCountDownTimer)].transform.position = Vector3.Lerp(myCountDownText[Mathf.CeilToInt(myCountDownTime - myCountDownTimer)].transform.position, myCountDownTextContainer.transform.position + new Vector3(2000, 2000, 0), 0.1f);


                    }
                }
            }

            Time.timeScale = 0;
            myCountDownTimer += Time.unscaledDeltaTime;
        }


    }
    public void LevelComplete()
    {
        myPlayer.transform.position = startPos;
        myCamera.ResetCameraPosition();
        myGrappleHook.enabled = false;
        myPlayerMovement.enabled = false;
        myLevelCompleteScreen.SetActive(true);
        myTotalTimeText.text = myScoreManager.TotalTime.ToString("0.00");
    }
    public void GameOver()
    {
        myLevelManager.ResetLevel();
        myCamera.ResetCameraPosition();
        AudioManager.ourPublicInstance.PlaySFX1(myDeathSoundClip, 1);
        myGrappleHook.enabled = false;
        myPlayerMovement.enabled = false;
        if (myGameOverScreen != null)
        {
            myGameOverScreen.SetActive(true);

        }
        myTotalTimeText.text = myScoreManager.TotalTime.ToString("0.00");

    }
    public Vector3 PlayerPosition()
    {
        return myPlayer.transform.position;
    }
    void StartGame()
    {
        Time.timeScale = 1;

        if (myCountDownTimer < myCountDownTime + 1)
        {
            myCountDownTimer += Time.unscaledDeltaTime;

        }
        else
        {
            if (myCountDownText != null)
            {
                for (int i = 0; i < myCountDownText.Length; i++)
                {
                    myCountDownTextContainer.SetActive(false);
                }
            }


        }

        if (myScoreManager != null)
        {
            myScoreManager.StartCounter = true;
        }



    }
    public void ResetGame(bool aResetTimer)
    {
        myGrappleHook.BreakHook();
        myLevelManager.ResetLevel();
        myPlayer.myCurrentHealth = myPlayer.myMaxHlaeth;
        if (aResetTimer)
        {
            myScoreManager.ResetTimer();

        }

        if (myGameOverScreen != null)
        {
            myGameOverScreen.SetActive(false);

        }
        if (myGameOverScreen != null)
        {
            myLevelCompleteScreen.SetActive(false);
        }

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}