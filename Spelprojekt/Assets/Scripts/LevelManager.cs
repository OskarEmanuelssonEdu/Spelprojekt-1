using System.Collections.Generic;
using UnityEngine;

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
    private Vector3 myStartPosition;
    [SerializeField]
    [Tooltip("THIS WILL BE AUTOMATICALLY FILLED")]
    private NewCameraMovement myCameraMovement;

    public Vector3 MyStartPosition
    {
        get { return myStartPosition; }
        set
        {
            myStartPosition = value;
            Debug.Log("New checkpoint!");
        }
    }

    // PRIVATE VARIABLES
    List<ObjectFalling> myFallingObjects;

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

        myStartPosition = myPlayerMovement.transform.position;
        myCameraMovement = FindObjectOfType<NewCameraMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        myFallingObjects = new List<ObjectFalling>();
        foreach (var fallingObject in FindObjectsOfType<ObjectFalling>())
        {
            myFallingObjects.Add(fallingObject);
        }
    }

    public void LevelComplete()
    {
        // TODO: Implement this
    }
    public void GameOver()
    {
        // TODO: Implement this
        // COUNTER: Should this be implemented here?
    }
    public void ResetLevel()
    {
        // Reset FallingObjects
        for (int index = 0; index < myFallingObjects.Count; index++)
        {
            myFallingObjects[index].ResetMe();
        }

        // Reset Camera
        myCameraMovement.ResetCameraPosition();

        // Code derived from GameManager
        // Date: 2020-10-08 16:24 UTC+1
        myPlayerMovement.CurrentSpeed = Vector3.zero;
        myPlayer.transform.position = myStartPosition;
        myPlayerMovement.enabled = true;
        myGrappleHook.enabled = true;
        myScoreManager.ResetTimer();
    }
}
