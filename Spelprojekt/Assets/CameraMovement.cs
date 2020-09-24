using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Camera myCamera;
    [SerializeField]
    private GameObject myPlayer;

    [Header("SPEED Settings")]
    [SerializeField]
<<<<<<< Updated upstream:Spelprojekt/Assets/CameraMovement.cs
    [Range(0f, 100f)]
    private float myMinMovementSpeed;
    [SerializeField]
    [Range(0f, 100f)]
=======
    [Range(0f, 5f)]
    private float myOverallSpeed;
    [SerializeField]
    [Range(0f, 100f)]
    private float myMinMovementSpeed;
    [SerializeField]
    [Range(0f, 100f)]
>>>>>>> Stashed changes:Spelprojekt/Assets/Scripts/CameraMovement.cs
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
    [Range(0f, 50f)]
    private float myMinFieldOfView;
    [SerializeField]
    [Range(0f, 50f)]
    private float myMaxFieldOfView;
<<<<<<< Updated upstream:Spelprojekt/Assets/CameraMovement.cs
    
=======

>>>>>>> Stashed changes:Spelprojekt/Assets/Scripts/CameraMovement.cs


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
<<<<<<< Updated upstream:Spelprojekt/Assets/CameraMovement.cs
        
=======

>>>>>>> Stashed changes:Spelprojekt/Assets/Scripts/CameraMovement.cs
        bool isTouchingLeftBound = screenPoint.x < myDistanceAllowedFromEdge;
        bool isTouchingZoomOutBound = screenPoint.x > myDistanceBeforeZoomingOut;
        bool isTouchingSpeedUpBound = screenPoint.x > myDistanceBeforeSpeedingUp;
        bool isTouchingSlowDownBound = screenPoint.x < myDistanceBeforeSlowingDown;

        if (isTouchingLeftBound)
        {
            //Kill Player
            Debug.Log("Touching Left Bound");
        }
        if (isTouchingZoomOutBound)
        {
            Debug.Log("Touching ZoomOut Bound");
            ZoomOut();
        }
<<<<<<< Updated upstream:Spelprojekt/Assets/CameraMovement.cs
        
=======

>>>>>>> Stashed changes:Spelprojekt/Assets/Scripts/CameraMovement.cs
        if (isTouchingSpeedUpBound)
        {
            Debug.Log("Touching SpeedUp Bound");
            ZoomOut();
<<<<<<< Updated upstream:Spelprojekt/Assets/CameraMovement.cs
            myCurrentMovementSpeed += myCameraFollowAcceleration * Time.deltaTime * screenPoint.x;
=======
            myCurrentMovementSpeed += myCameraFollowAcceleration * Time.deltaTime * (screenPoint.x)* (screenPoint.x) * myOverallSpeed;
>>>>>>> Stashed changes:Spelprojekt/Assets/Scripts/CameraMovement.cs
            if (myCurrentMovementSpeed > myMaxMovementSpeed)
            {
                myCurrentMovementSpeed = myMaxMovementSpeed;
            }
        }
        if (isTouchingSlowDownBound)
        {
            ZoomIn();
<<<<<<< Updated upstream:Spelprojekt/Assets/CameraMovement.cs
            myCurrentMovementSpeed -= myCameraFollowDeceleration * Time.deltaTime;
=======
            //Camera will deccelerate faster the closer the player gets to the edge of the screen
            myCurrentMovementSpeed -= myCameraFollowDeceleration * Time.deltaTime * (1-screenPoint.x)* (1 - screenPoint.x) * myOverallSpeed;
>>>>>>> Stashed changes:Spelprojekt/Assets/Scripts/CameraMovement.cs
            if (myCurrentMovementSpeed < myMinMovementSpeed)
            {
                myCurrentMovementSpeed = myMinMovementSpeed;
            }
            Debug.Log("Touching SlowDown Bound");
        }
    }
    private void Move()
    {
<<<<<<< Updated upstream:Spelprojekt/Assets/CameraMovement.cs
        
        transform.Translate(new Vector3(myCurrentMovementSpeed * Time.deltaTime, 0, 0));

    }
    private void ZoomIn()
    {
        myCamera.orthographicSize -= myZoomInSpeed * Time.deltaTime;
=======

        transform.Translate(new Vector3(myCurrentMovementSpeed * Time.deltaTime * myOverallSpeed, 0, 0));

    }
    private void ZoomIn()
    {
        myCamera.orthographicSize -= myZoomInSpeed * Time.deltaTime * myOverallSpeed;
>>>>>>> Stashed changes:Spelprojekt/Assets/Scripts/CameraMovement.cs
        if (myCamera.orthographicSize < myMinFieldOfView)
        {
            myCamera.orthographicSize = myMinFieldOfView;
        }
    }

    private void ZoomOut()
    {
<<<<<<< Updated upstream:Spelprojekt/Assets/CameraMovement.cs
        myCamera.orthographicSize += myZoomOutSpeed * Time.deltaTime;
=======
        myCamera.orthographicSize += myZoomOutSpeed * Time.deltaTime * myOverallSpeed;
>>>>>>> Stashed changes:Spelprojekt/Assets/Scripts/CameraMovement.cs
        if (myCamera.orthographicSize > myMaxFieldOfView)
        {
            myCamera.orthographicSize = myMaxFieldOfView;
        }
    }
}
