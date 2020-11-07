using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using TMPro;
public class LevelManager : MonoBehaviour
{
    // Singleton pattern
    private static LevelManager myInstance;
    public static LevelManager ourInstance
    {
        get { return myInstance; }
    }

    // SERIALIZEFIELDS
    [SerializeField]
    [Tooltip("THIS WILL BE AUTOMATICALLY FILLED")]
    private Player myPlayer;
    [SerializeField]
    [Tooltip("THIS WILL BE AUTOMATICALLY FILLED")]
    private PlayerMovement myPlayerMovement;
    [SerializeField]
    [Tooltip("THIS WILL BE AUTOMATICALLY FILLED")]
    private GrappleHookBoohyah myGrappleHook;
    [SerializeField]
    [Tooltip("THIS WILL BE AUTOMATICALLY FILLED")]
    private ScoreManager myScoreManager;
    [SerializeField]
    [Tooltip("THIS WILL BE AUTOMATICALLY FILLED")]
    private Vector3 myPlayerPosition;
    [SerializeField]
    [Tooltip("THIS WILL BE AUTOMATICALLY FILLED")]
    private Vector3 myCameraPosition;
    [SerializeField]
    [Tooltip("THIS WILL BE AUTOMATICALLY FILLED")]
    private NewCameraMovement myCameraMovement;
    [SerializeField]
    private GameObject myPlayerModel;
    [SerializeField]
    TextMeshProUGUI myTotalScore;


    [Header("Pause screen")]
    [SerializeField]
    GameObject myPauseMenu;
    [Header("End screen")]
    [SerializeField]
    GameObject myEndScreen;
    [SerializeField]
    LeverLoader myLevelLoader;

    [SerializeField]
    private VisualEffect myDeathEffect;

    [SerializeField]
    [Tooltip("Defines the time it takes to reset player after they died")]
    [Min(0.0f)]
    private float myResetDelayTime;

    public Vector3 MyStartPosition
    {
        get { return myPlayerPosition; }
        set
        {
            myPlayerPosition = value;
            myCameraMovement.ChangeCameraResetPosition(value);
          
        }
    }

    // PRIVATE VARIABLES
    List<ObjectFalling> myFallingObjects;
    private float myClock;
    private bool myLevelIsGoingToReset;

    // Singleton pattern
    private void Awake()
    {
        if (myInstance != null && myInstance != this)
        {
            Destroy(this);
        }
        else
        {
            myInstance = this;
        }
    }

    private void OnValidate()
    {
        myPlayer = FindObjectOfType<Player>();
        myPlayerMovement = FindObjectOfType<PlayerMovement>();
        myGrappleHook = FindObjectOfType<GrappleHookBoohyah>();
        myScoreManager = FindObjectOfType<ScoreManager>();
        myLevelLoader = FindObjectOfType<LeverLoader>();

        myPlayerPosition = myPlayerMovement.transform.position;
        myCameraMovement = FindObjectOfType<NewCameraMovement>();
        myCameraPosition = myCameraMovement.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        myClock = 0.0f;
        myLevelIsGoingToReset = false;
        myFallingObjects = new List<ObjectFalling>();
        foreach (var fallingObject in FindObjectsOfType<ObjectFalling>())
        {
            myFallingObjects.Add(fallingObject);
        }

    }
    public void ReturnToMainMenu()
    {
        Unpause();
        print("Should unpause");
        SceneManager.LoadScene(0);

    }

    public void LevelComplete()
    {
        // TODO: Implement this
        myTotalScore.text = myScoreManager.TotalTime.ToString("0.00");
        myEndScreen.SetActive(true);
        Time.timeScale = 0;


    }
    public void Nextlevel()
    {
        Unpause();
        AudioManager.ourPublicInstance.IncreaseMusicIndex();
        myLevelLoader.LoadNextLevel();

    }
    public void GameOver()
    {
        // TODO: Implement this
        // COUNTER: Should this be implemented here?
    }

    public void Update()
    {
        if (myLevelIsGoingToReset)
        {
            // Tick the clock / Increase the internal time
            myClock += Time.deltaTime;

            if (myClock >= myResetDelayTime)
            {
                myClock = 0.0f;
                InternalResetLevel();
            }
        }
    }

    public void ResetLevel()
    {
        if (!myLevelIsGoingToReset)
        {
            Unpause();

            myLevelIsGoingToReset = true;

            myPlayerMovement.enabled = false;

            myPlayerModel.SetActive(false);
            myDeathEffect.transform.position = myPlayerMovement.transform.position;
            myDeathEffect.SendEvent("PlayDeathEffect");
        }
    }

    private void InternalResetLevel()
    {
        myLevelIsGoingToReset = false;

        // Reset FallingObjects
        for (int index = 0; index < myFallingObjects.Count; index++)
        {
            myFallingObjects[index].ResetMe();
        }

        // Reset Camera

        // Code derived from GameManager
        // Date: 2020-10-08 16:24 UTC+1
        myPlayerMovement.CurrentSpeed = Vector3.zero;
        myPlayer.transform.position = myPlayerPosition;
        myPlayerMovement.enabled = true;
        myGrappleHook.enabled = true;
        myPlayerModel.SetActive(true);
        myCameraMovement.ResetCameraPosition();

    }

    public void Pause()
    {
        Time.timeScale = 0f;
        myPauseMenu.SetActive(true);

    }
    public void Unpause()
    {
        Time.timeScale = 1;
        myPauseMenu.SetActive(false);

    }

}
