using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float myMovementSpeed;
    [SerializeField]
    private float myMaxMovementSpeed;
    private float myCurrentMovementSpeed;
    [SerializeField]
    private GameObject myPlayer;

    [Header("All the way to the left is 0, to the right is 100")]
    //How close to the edge of the screen can the player get before dying
    [SerializeField]
    [Range(0f, 100f)]
    private float myDistanceAllowedFromEdge;
    //How far to the right can the player run before the camera starts zooming out
    [SerializeField]
    [Range(0f, 100f)]
    private float myDistanceBeforeZoomingOut;
    [SerializeField]
    [Range(0f, 100f)]
    private float myDistanceBeforeSpeedingUp;
    [SerializeField]
    [Range(0f, 100f)]
    private float myDistanceBeforeSlowingDown;


    [Header("FOV Settings")]
    [SerializeField]
    private float myZoomOutSpeed;
    [SerializeField]
    private float myZoomInSpeed;
    //If Camera is ortographic, change to size instead of FOV
    [SerializeField]
    [Range(0f, 150f)]
    private float myMinFieldOfView;
    [SerializeField]
    [Range(0f, 150f)]
    private float myMaxFieldOfView;
    //How fast does move to the player when zooming out
    [SerializeField]
    [Range (0f, 10f)]
    private float myCameraFollowZoomOutSpeed;
    [SerializeField]
    Camera myCamera;


    private void Start()
    {
        myCurrentMovementSpeed = myMovementSpeed;
    }
    void Update()
    {
        Debug.Log("Camera Speed: " + myCurrentMovementSpeed);
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
        
        bool isTouchingLeftBound = screenPoint.x < myDistanceAllowedFromEdge/100;
        bool isTouchingZoomOutBound = screenPoint.x > myDistanceBeforeZoomingOut/100;

        bool isTouchingSpeedUpBound = screenPoint.x > myDistanceBeforeSpeedingUp / 100;
        bool isTouchingSlowDownBound = screenPoint.x < myDistanceBeforeSlowingDown / 100;

        if (isTouchingLeftBound)
        {
            //Kill Player
            //Destroy(myPlayer);
            Debug.Log("Touching Left Bound");
        }
        if (isTouchingZoomOutBound)
        {
            Debug.Log("Touching ZoomOut Bound");
            ZoomOut();
        }
        if ((myCamera.fieldOfView > myMinFieldOfView))
        {
            ZoomIn();
        }
        if (isTouchingSpeedUpBound)
        {
            Debug.Log("Touching SpeedUp Bound");
            myCurrentMovementSpeed = (int)Mathf.Lerp(myCurrentMovementSpeed, myMaxMovementSpeed, myCameraFollowZoomOutSpeed * Time.deltaTime);
           
        }
        if(isTouchingSlowDownBound)
        {
            Debug.Log("Touching SlowDown Bound");
            myCurrentMovementSpeed = (int)Mathf.Lerp(myCurrentMovementSpeed, myMovementSpeed, myCameraFollowZoomOutSpeed * Time.deltaTime);
        }

        

    }
    private void Move()
    {
        transform.Translate(new Vector3(myCurrentMovementSpeed * Time.deltaTime, 0, 0));


    }
    private void ZoomIn()
    {
        myCamera.fieldOfView -= myZoomInSpeed * Time.deltaTime;
    }

    private void ZoomOut()
    {
        myCamera.fieldOfView += myZoomOutSpeed * Time.deltaTime;
        
    }
}
