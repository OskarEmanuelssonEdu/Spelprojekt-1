using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float myMovementSpeed;
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
        
    }
    void Update()
    {
        Move();
        CheckPlayerScreenLocation();
        if (myCamera.fieldOfView > myMaxFieldOfView)
        {
            myCamera.fieldOfView = myMaxFieldOfView;
        }
    }

    private void Move()
    {
        transform.Translate(new Vector3(myMovementSpeed * Time.deltaTime, 0, 0));
    }

    private void CheckPlayerScreenLocation()
    {
        Vector3 screenPoint = myCamera.WorldToViewportPoint(myPlayer.transform.position);
        
        bool isTouchingLeftBound = screenPoint.x < myDistanceAllowedFromEdge/100;
        bool isTouchingRightBound = screenPoint.x > myDistanceBeforeZoomingOut/100;

        if (isTouchingLeftBound)
        {
            //Kill Player
            //Destroy(myPlayer);
            Debug.Log("Touching Left Bound");
        }
        if (isTouchingRightBound)
        {
            Debug.Log("Touching Left Bound");
            ZoomOut();
        }
        if (!isTouchingRightBound && (myCamera.fieldOfView > myMinFieldOfView))
        {
            ZoomIn();
        }

    }

    private void ZoomIn()
    {
        myCamera.fieldOfView -= myZoomInSpeed * Time.deltaTime;
        Vector3 pointToMoveTo = new Vector3(myPlayer.transform.position.x, transform.position.y, transform.position.z);
    }

    private void ZoomOut()
    {
        myCamera.fieldOfView += myZoomOutSpeed * Time.deltaTime;
        Vector3 pointToMoveTo = new Vector3(myPlayer.transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, pointToMoveTo, Time.deltaTime * myCameraFollowZoomOutSpeed);
    }
}
