using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    
    [SerializeField]
    ScoreManager myScoreManager;
    [SerializeField]
    PlayerMovement myPlayerMovement;
    [SerializeField]
    GrappleHookBoohyah grappleHook;
    [SerializeField]
    Player myPlayer;
    Vector3 startPos;

    [SerializeField]
    NewCameraMovement myCamera;
   
    void OnValidate()
    {
        myPlayer = FindObjectOfType<Player>();
        myPlayerMovement = FindObjectOfType<PlayerMovement>();
        grappleHook = FindObjectOfType<GrappleHookBoohyah>();
        myScoreManager = FindObjectOfType<ScoreManager>();
        myCamera = FindObjectOfType<NewCameraMovement>();
    }
    void Start()
    {
        grappleHook.enabled = true;
        myPlayerMovement.enabled = true;
        startPos = myPlayer.transform.position;        
    }
    void Update()
    {
        CountDownToStart();
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
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
    public void LevelComplete()
    {
        myPlayer.transform.position = startPos;

        grappleHook.enabled = false;
        myPlayerMovement.enabled = false;
        myLevelCompleteScreen.SetActive(true);
        myTotalTimeText.text = myScoreManager.TotalTime.ToString("0.00");
    }
    public void GameOver()
    {
        myPlayer.transform.position = startPos;
        myCamera.ResetCameraPosition();
        grappleHook.enabled = false;
        myPlayerMovement.enabled = false;
        //myGameOverScreen.SetActive(true);
        //myTotalTimeText.text = myScoreManager.TotalTime.ToString("0.00");
        

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
    public void ResetGame()
    {
        
        grappleHook.enabled = true;
        myPlayerMovement.enabled = true;
        myGameOverScreen.SetActive(false);
        myLevelCompleteScreen.SetActive(false);
        myScoreManager.ResetTimer(); 
        myCamera.ResetCameraPosition();

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
