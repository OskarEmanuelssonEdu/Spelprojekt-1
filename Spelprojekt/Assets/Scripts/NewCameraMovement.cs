using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraMovement : MonoBehaviour
{


    [SerializeField]
    Camera myCamera;
    [SerializeField]
    private Player myPlayer;

    [Header("BOUNDARY Settings")]
    [SerializeField]
    [Range(0f, 900)]
    private int myPixelsAllowedFromRight = 200;
    [SerializeField]
    [Range(0f, 900)]
    private int myPixelsAllowedFromLeft = 200;
    [SerializeField]
    [Range(0f, 500)]
    private int myPixelsAllowedFromUp = 100;
    [SerializeField]
    [Range(0f, 500)]
    private int myPixelsAllowedFromDown = 100;

    [Header("POSITION Settings")]
    [SerializeField]
    [Range(-30f, 30f)]
    private float myPlayerHorizontalCameraPosition = -5f;
    [SerializeField]
    [Range(-20f, 20f)]
    private float myPlayerVerticalCameraPosition = -8f;


    [Header("FOV Settings")]
    [SerializeField]
    [Range(0f, 3f)]
    private float myZoomOutSpeed = 0.1f;
    [SerializeField]
    [Range(0f, 20f)]
    private float myZoomInSpeed = 5f;
    [SerializeField]
    [Range(0f, 200f)]
    private float myMinFieldOfView = 70f;
    [SerializeField]
    [Range(0f, 200f)]
    private float myMaxFieldOfView = 80f;

    [Header("SPEED Settings")]
    [SerializeField]
    [Range(0f, 3f)]
    private float myVerticalSpeed = 0.5f;
    [SerializeField]
    [Range(0f, 3f)]
    private float myHorizontalSpeed = 1f;


    Vector3 myPlayerCurrentVelocity;
    Vector3 myPlayerPrevPos;
    Vector3 myPlayerNewPos;

    Vector3 positionToMoveTo;
    Vector3 screenPoint;

    private void Start()
    {
        if (myCamera == null)
        {
            myCamera = Camera.main;
        }
        myPlayerPrevPos = myPlayer.transform.position;

    }
    private void OnValidate()
    {
        myPlayer = FindObjectOfType<Player>();
    }
    void FixedUpdate()
    {
        CheckPlayerVelocity();
        //Debug.Log("Player velocity: " + myPlayerCurrentVelocity);

        ZoomOut();
        if (Mathf.Abs(myPlayerCurrentVelocity.x) < 1)
        {
            ZoomIn();
        }
        Move(screenPoint);
        CheckPlayerScreenLocation();
    }

    private void CheckPlayerVelocity()
    {
        myPlayerNewPos = myPlayer.transform.position;
        myPlayerCurrentVelocity = ((myPlayerNewPos - myPlayerPrevPos) / Time.deltaTime) * 0.5f;
        myPlayerPrevPos = myPlayerNewPos;
    }

    private void Move(Vector3 aScreenpoint)
    {
        positionToMoveTo = new Vector3(myPlayer.transform.position.x + (myPlayerCurrentVelocity.x * myHorizontalSpeed) - myPlayerHorizontalCameraPosition,
                                        myPlayer.transform.position.y + (myPlayerCurrentVelocity.y * myVerticalSpeed) - myPlayerVerticalCameraPosition,
                                        transform.position.z);
        if (aScreenpoint.x > myPixelsAllowedFromLeft && aScreenpoint.x < Screen.width - myPixelsAllowedFromRight)
        {

            transform.position = Vector3.Lerp(transform.position, new Vector3(positionToMoveTo.x, transform.position.y, transform.position.z), Time.deltaTime);
        }
        else
        {

            transform.position = Vector3.Lerp(transform.position, new Vector3(myPlayer.transform.position.x, transform.position.y, transform.position.z), Time.deltaTime / 1000);

        }
        if (aScreenpoint.y > myPixelsAllowedFromDown && screenPoint.y < Screen.height - myPixelsAllowedFromUp)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, positionToMoveTo.y, transform.position.z), Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, myPlayer.transform.position.y, transform.position.z), Time.deltaTime / 1000);

        }



    }

    private void CheckPlayerScreenLocation()
    {
        screenPoint = myCamera.WorldToScreenPoint(myPlayer.transform.position);
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
        myCamera.fieldOfView += Mathf.Abs(myPlayerCurrentVelocity.x) * myZoomOutSpeed * Time.deltaTime;
        if (myCamera.fieldOfView > myMaxFieldOfView)
        {
            myCamera.fieldOfView = myMaxFieldOfView;
        }
    }

}
