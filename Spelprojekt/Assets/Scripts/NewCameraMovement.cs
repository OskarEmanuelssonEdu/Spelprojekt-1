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
    Vector3 myPlayerScreenPoint;
    Vector3 myCameraStartPosition;

    Vector3 bld2;

    private void Start()
    {
        if (myCamera == null)
        {
            myCamera = Camera.main;
        }
        myPlayerPrevPos = myPlayer.transform.position;
        myCameraStartPosition = transform.position;
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
        Move();
        CheckPlayerScreenLocation();


    }
    public void ResetCameraPosition()
    {
        transform.position = myCameraStartPosition;
    }
    private void CheckPlayerVelocity()
    {
        myPlayerNewPos = myPlayer.transform.position;
        myPlayerCurrentVelocity = ((myPlayerNewPos - myPlayerPrevPos) / Time.deltaTime) * 0.5f;
        myPlayerPrevPos = myPlayerNewPos;
    }

    private void Move()
    {
        positionToMoveTo = new Vector3(myPlayer.transform.position.x + (myPlayerCurrentVelocity.x * myHorizontalSpeed) - myPlayerHorizontalCameraPosition,
                                        myPlayer.transform.position.y + (myPlayerCurrentVelocity.y * myVerticalSpeed) - myPlayerVerticalCameraPosition,
                                        transform.position.z);

        Vector3 targetPosition = transform.position;
        Vector3 temp;

        if (myPlayerScreenPoint.x > myPixelsAllowedFromLeft && myPlayerScreenPoint.x < Screen.width - myPixelsAllowedFromRight)
        {
            targetPosition.x = positionToMoveTo.x;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
        }
        else if (myPlayerScreenPoint.x < myPixelsAllowedFromLeft)
        {
            temp = myCamera.ScreenToWorldPoint(new Vector3(myPixelsAllowedFromLeft, myPlayerScreenPoint.y, myPlayerScreenPoint.z));
            targetPosition.x = temp.x;
            transform.position = Vector3.Lerp(transform.position, targetPosition,  0.01f *Time.deltaTime);
        }
        else if (myPlayerScreenPoint.x > Screen.width - myPixelsAllowedFromRight)
        {
            temp = myCamera.ScreenToWorldPoint(new Vector3(Screen.width - myPixelsAllowedFromRight, myPlayerScreenPoint.y, myPlayerScreenPoint.z));
            targetPosition.x = temp.x;
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.01f * Time.deltaTime);
        }
        if (myPlayerScreenPoint.y > myPixelsAllowedFromDown && myPlayerScreenPoint.y < Screen.height - myPixelsAllowedFromUp)
        {
            targetPosition.y = positionToMoveTo.y;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
        }
        else if (myPlayerScreenPoint.y < myPixelsAllowedFromDown)
        {
            temp = myCamera.ScreenToWorldPoint(new Vector3(myPlayerScreenPoint.y, myPixelsAllowedFromDown, myPlayerScreenPoint.z));
            targetPosition.y = temp.y;
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.01f * Time.deltaTime);
        }
        else if (myPlayerScreenPoint.y > Screen.height - myPixelsAllowedFromUp)
        {
            temp = myCamera.ScreenToWorldPoint(new Vector3(myPlayerScreenPoint.y, Screen.height - myPixelsAllowedFromUp, myPlayerScreenPoint.z));
            targetPosition.y = temp.y;
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.01f * Time.deltaTime);
        }
    }
    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        CheckPlayerScreenLocation();

        Vector3 screenie = myCamera.ScreenToWorldPoint(myPlayerScreenPoint);
        Vector3 bld = myCamera.ScreenToWorldPoint(new Vector3(myPixelsAllowedFromLeft, myPixelsAllowedFromDown, -transform.position.z));
        Vector3 bru = myCamera.ScreenToWorldPoint(new Vector3(Screen.width - myPixelsAllowedFromRight, Screen.height - myPixelsAllowedFromUp, -transform.position.z));

        Gizmos.color = Color.cyan;
        // ScreenPoint X
        Gizmos.DrawLine(new Vector3(screenie.x, -500, 0), new Vector3(screenie.x, 500, 0));
        // ScreenPoint Y
        Gizmos.DrawLine(new Vector3(-500, screenie.y, 0), new Vector3(500, screenie.y, 0));
        Gizmos.color = Color.red;
        // Boundary Left
        Gizmos.DrawLine(new Vector3(bld.x, -500, 0), new Vector3(bld.x, 500, 0));
        // Boundary Right
        Gizmos.DrawLine(new Vector3(bru.x, -500, 0), new Vector3(bru.x, 500, 0));
        // Boundary Down
        Gizmos.DrawLine(new Vector3(-500, bld.y, 0), new Vector3(500, bld.y, 0));
        // Boundary Up
        Gizmos.DrawLine(new Vector3(-500, bru.y, 0), new Vector3(500, bru.y, 0));
    }

    private void CheckPlayerScreenLocation()
    {
        myPlayerScreenPoint = myCamera.WorldToScreenPoint(myPlayer.transform.position);
        Vector3 bld2 = myCamera.ScreenToWorldPoint(new Vector3(myPixelsAllowedFromLeft, myPlayerScreenPoint.y, -transform.position.z));
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
