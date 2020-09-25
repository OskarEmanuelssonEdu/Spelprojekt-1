using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private AudioClip myAudioWhenNearScreen;
    [SerializeField]
    private AudioClip myAudioDeath;

    [SerializeField]
    Camera myCamera;
    [SerializeField]
    private GameObject myPlayer;

    [Header("SPEED Settings")]
    [SerializeField]
    [Range(0f, 100f)]
    private float myMinMovementSpeed;
    [SerializeField]
    [Range(0f, 100f)]
    private float myMaxMovementSpeed;
    [SerializeField]
    [Range(0f, 100f)]
    private float myCurrentMovementSpeed;
    [SerializeField]
    [Range(0f, 100f)]
    private float myCameraFollowAcceleration;
    [SerializeField]
    [Range(0f, 100f)]
    private float myCameraFollowDeceleration;



    [Header("BOUNDARY Settings")]
    [Header("All the way to the left is 0, to the right is 1")]
    //How close to the edge of the screen can the player get before dying
    [SerializeField]
    [Range(0f, 1f)]
    private float myDistanceAllowedFromLeftEdge;
    //How far to the right can the player run before the camera starts zooming out
    [SerializeField]
    [Range(0f, 1f)]
    private float myDistanceToRightBeforeZoomingOut;
    [SerializeField]
    [Range(0f, 1f)]
    private float myDistanceUpBeforeZoomingOut;
    [SerializeField]
    [Range(0f, 1f)]
    private float myDistanceToRightBeforeSpeedingUp;
    [SerializeField]
    [Range(0f, 1f)]
    private float myDistanceUpBeforeSpeedingUp;
    [SerializeField]
    [Range(0f, 1f)]
    private float myDistanceFromLeftBeforeSlowingDown;
    [SerializeField]
    [Range(0f, 1f)]
    private float myDistanceToRightBeforeSuperSpeedingUp;
    [SerializeField]
    [Range(-10f, 10f)]
    private float myPlayerVerticalCameraPosition;


    [Header("FOV Settings")]
    [SerializeField]
    [Range(0f, 50f)]
    private float myZoomOutSpeed;
    [SerializeField]
    [Range(0f, 50f)]
    private float myZoomInSpeed;
    [SerializeField]
    [Range(0f, 200f)]
    private float myMinFieldOfView;
    [SerializeField]
    [Range(0f, 200f)]
    private float myMaxFieldOfView;



    bool touchedSlowDownBoundLastFrame = false;
    bool touchedLeftBoundLastFrame = false;
    bool touchedSpeedUpBoundLastFrame = false;

    private void Start()
    {
        
        myCurrentMovementSpeed = myMinMovementSpeed;
    }
    void Update()
    {
        CheckPlayerScreenLocation();
        Move();

        if (myCamera.orthographicSize > myMaxFieldOfView)
        {
            myCamera.orthographicSize = myMaxFieldOfView;
        }
    }

    private void CheckPlayerScreenLocation()
    {
        Vector3 screenPoint = myCamera.WorldToViewportPoint(myPlayer.transform.position);
        Vector3 playerWorldPoint = myCamera.ViewportToWorldPoint(myPlayer.transform.position);

        bool isTouchingLeftBound = screenPoint.x < myDistanceAllowedFromLeftEdge;
        bool isTouchingRightZoomOutBound = screenPoint.x > myDistanceToRightBeforeZoomingOut;
        bool isTouchingRightSpeedUpBound = screenPoint.x > myDistanceToRightBeforeSpeedingUp;
        bool isTouchingLeftSlowDownBound = screenPoint.x < myDistanceFromLeftBeforeSlowingDown;

        bool isTouchingRightSuperSpeedUpBound = screenPoint.x > myDistanceToRightBeforeSuperSpeedingUp;
        LeftBound(isTouchingLeftBound);
        RightZoomOutBound(isTouchingRightZoomOutBound);

        RightSpeedUpBound(screenPoint, isTouchingRightSpeedUpBound);
        if (screenPoint.x > myDistanceToRightBeforeSuperSpeedingUp)
        {
            float positionDifference = myPlayer.transform.position.x - transform.position.x;
            transform.position += new Vector3(10/positionDifference,0,0);
            Debug.Log("Touching Super Speed");
        }
        LeftSlowDownBound(isTouchingLeftSlowDownBound);
    }

    private void RightZoomOutBound(bool isTouchingRightZoomOutBound)
    {
        if (isTouchingRightZoomOutBound)
        {
            Debug.Log("Touching ZoomOut Bound");
            ZoomOut();
        }
    }

    private void RightSpeedUpBound(Vector3 screenPoint, bool isTouchingSpeedUpBound)
    {
        if (isTouchingSpeedUpBound)
        {
            if (!touchedSpeedUpBoundLastFrame)
            {
                //AudioManager.Instance.FadeOutMusicVolume(2, 0.1f);
            }
            touchedSpeedUpBoundLastFrame = true;

            Debug.Log("Touching SpeedUp Bound");
            ZoomOut();
            myCurrentMovementSpeed += myCameraFollowAcceleration * Time.deltaTime * screenPoint.x;
            if (myCurrentMovementSpeed > myMaxMovementSpeed)
            {
                myCurrentMovementSpeed = myMaxMovementSpeed;
            }
        }
        else
        {
            touchedSpeedUpBoundLastFrame = false;
        }
    }

    private void LeftBound(bool isTouchingLeftBound)
    {
        if (isTouchingLeftBound)
        {
            if (!touchedLeftBoundLastFrame)
            {
                AudioManager.Instance.PlaySFX(myAudioDeath);
            }
            //Kill Player
            Debug.Log("Touching Left Bound");
            touchedLeftBoundLastFrame = true;
        }
        else
        {
            touchedLeftBoundLastFrame = false;
        }
    }

    private void LeftSlowDownBound(bool isTouchingSlowDownBound)
    {
        
        if (isTouchingSlowDownBound)
        {
            if (!touchedSlowDownBoundLastFrame)
            {
                //AudioManager.Instance.FadeInMusicVolume(2f, 0.6f);
            }
            


            ZoomIn();
            myCurrentMovementSpeed -= myCameraFollowDeceleration * Time.deltaTime;
            if (myCurrentMovementSpeed < myMinMovementSpeed)
            {
                myCurrentMovementSpeed = myMinMovementSpeed;
            }
            Debug.Log("Touching SlowDown Bound");
            touchedSlowDownBoundLastFrame = true;
        }
        else
        {
            //AudioManager.Instance.FadeOutMusicVolume(2, 0.1f);
            touchedSlowDownBoundLastFrame = false;
        }
    }

    private void Move()
    {

        transform.Translate(new Vector3(myCurrentMovementSpeed * Time.deltaTime, 0, 0));
        transform.position = new Vector3(transform.position.x, myPlayer.transform.position.y-myPlayerVerticalCameraPosition, transform.position.z);

    }
    private void ZoomIn()
    {
        myCamera.orthographicSize -= myZoomInSpeed * Time.deltaTime;
        if (myCamera.orthographicSize < myMinFieldOfView)
        {
            myCamera.orthographicSize = myMinFieldOfView;
        }
    }

    private void ZoomOut()
    {
        myCamera.orthographicSize += myZoomOutSpeed * Time.deltaTime;
        if (myCamera.orthographicSize > myMaxFieldOfView)
        {
            myCamera.fieldOfView = myMaxFieldOfView;
        }
    }
}
