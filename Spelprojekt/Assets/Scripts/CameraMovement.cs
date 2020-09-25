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
    private float myDistanceAllowedFromEdge;
    //How far to the right can the player run before the camera starts zooming out
    [SerializeField]
    [Range(0f, 1f)]
    private float myDistanceBeforeZoomingOut;
    [SerializeField]
    [Range(0f, 1f)]
    private float myDistanceBeforeSpeedingUp;
    [SerializeField]
    [Range(0f, 1f)]
    private float myDistanceBeforeSlowingDown;


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

    private void Start()
    {
        AudioManager.Instance.PlayMusicWithFade(myAudioWhenNearScreen, 0.5f);
        myCurrentMovementSpeed = myMinMovementSpeed;
    }
    void Update()
    {
        CheckPlayerScreenLocation();
        Move();

        if (myCamera.fieldOfView > myMaxFieldOfView)
        {
            myCamera.fieldOfView = myMaxFieldOfView;
        }
    }

    private void CheckPlayerScreenLocation()
    {
        Vector3 screenPoint = myCamera.WorldToViewportPoint(myPlayer.transform.position);

        bool isTouchingLeftBound = screenPoint.x < myDistanceAllowedFromEdge;
        bool isTouchingZoomOutBound = screenPoint.x > myDistanceBeforeZoomingOut;
        bool isTouchingSpeedUpBound = screenPoint.x > myDistanceBeforeSpeedingUp;
        bool isTouchingSlowDownBound = screenPoint.x < myDistanceBeforeSlowingDown;
        LeftBound(isTouchingLeftBound);
        if (isTouchingZoomOutBound)
        {
            Debug.Log("Touching ZoomOut Bound");
            ZoomOut();
        }

        if (isTouchingSpeedUpBound)
        {
            Debug.Log("Touching SpeedUp Bound");
            ZoomOut();
            myCurrentMovementSpeed += myCameraFollowAcceleration * Time.deltaTime * screenPoint.x;
            if (myCurrentMovementSpeed > myMaxMovementSpeed)
            {
                myCurrentMovementSpeed = myMaxMovementSpeed;
            }
        }
        SlowDownBound(isTouchingSlowDownBound);
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

    private void SlowDownBound(bool isTouchingSlowDownBound)
    {
        
        if (isTouchingSlowDownBound)
        {
            if (!touchedSlowDownBoundLastFrame)
            {
                AudioManager.Instance.FadeInMusicVolume(2f, 0.6f);
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
            AudioManager.Instance.FadeOutMusicVolume(2, 0.1f);
            touchedSlowDownBoundLastFrame = false;
        }
    }

    private void Move()
    {

        transform.Translate(new Vector3(myCurrentMovementSpeed * Time.deltaTime, 0, 0));

    }
    private void ZoomIn()
    {
        myCamera.fieldOfView -= myZoomInSpeed * Time.deltaTime;
        if (myCamera.fieldOfView < myMinFieldOfView)
        {
            myCamera.fieldOfView = myMinFieldOfView;
        }
    }

    private void ZoomOut()
    {
        myCamera.fieldOfView += myZoomOutSpeed * Time.deltaTime;
        if (myCamera.fieldOfView > myMaxFieldOfView)
        {
            myCamera.fieldOfView = myMaxFieldOfView;
        }
    }
}
