using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float myMovementSpeed;
    [SerializeField]
    private GameObject myPlayer;

    //How close to the edge of the screen can the player get before dying
    [SerializeField]
    private float myDistanceAllowedFromEdge;
    //How far to the right can the player run before the camera starts zooming out
    [SerializeField]
    private float myDistanceBeforeZoomingOut;
    [SerializeField]
    private float myZoomOutSpeed;
    [SerializeField]
    private float myZoomInSpeed;
    [SerializeField]
    private float myMinFieldOfView;
    [SerializeField]
    private float myMaxFieldOfView;
    //How fast does move to the player when zooming in
    [SerializeField]
    private float myCameraFollowZoomInSmoothFactor;
    //How fast does move to the player when zooming out
    [SerializeField]
    private float myCameraFollowZoomOutSmoothFactor;
    [SerializeField]
    Camera myCamera;


    private void Start()
    {
        
    }
    void Update()
    {
        Debug.Log(transform.)
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
            Debug.Log("Player not on screen");
        }
        if (isTouchingRightBound)
        {
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
        transform.position = Vector3.Lerp(transform.position, pointToMoveTo, Time.deltaTime * myCameraFollowZoomInSmoothFactor);
    }

    private void ZoomOut()
    {
        myCamera.fieldOfView += myZoomOutSpeed * Time.deltaTime;
        Vector3 pointToMoveTo = new Vector3(myPlayer.transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, pointToMoveTo, Time.deltaTime * myCameraFollowZoomOutSmoothFactor);
    }
    

}
