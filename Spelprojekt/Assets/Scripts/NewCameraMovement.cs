﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraMovement : MonoBehaviour
{

    [SerializeField]
    float newFloat;
    [SerializeField]
    Camera myCamera;
    [SerializeField]
    Player myPlayer;
    [SerializeField]
    PlayerMovement myPlayerMovement;

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

    [Header("RESTING POSITION Settings")]
    [SerializeField]
    [Range(-10f, 10f)]
    private float myPlayerHorizontalCameraPosition = 0f;
    [SerializeField]
    [Range(-10f, 10f)]
    private float myPlayerVerticalCameraPosition = -3f;


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

    [Header("ACCELERATION Settings")]
    [SerializeField]
    [Range(0f, 2f)]
    private float myVerticalAcceleration = 0.3f;
    [SerializeField]
    [Range(0f, 2f)]
    private float myHorizontalAcceleration = 0.6f;


    Vector3 myPlayerCurrentVelocity;
    Vector3 myPlayerPrevPos;
    Vector3 myPlayerNewPos;

    Vector3 myPositionToMoveTo;
    Vector3 myPlayerScreenPoint;
    Vector3 myTargetPosition;
    Vector3 myBoundaryWorldPoint;
    Vector3 myCenterWorldPoint;

    Vector3 myCameraStartPosition;
    float myCameraStartFOV;

    [SerializeField]
    Transform myParentTransform;
    private void Start()
    {
        if (myCamera == null)
        {
            myCamera = Camera.main;
        }
        myPlayerPrevPos = myParentTransform.transform.position;
        myCameraStartPosition = transform.position;
        myCameraStartFOV = myCamera.fieldOfView;
    }
    private void OnValidate()
    {
        //myPlayer = FindObjectOfType<Player>();
        //myPlayerMovement = FindObjectOfType<PlayerMovement>();
        //myParentTransform = GetComponentInParent<Transform>();
    }
    void FixedUpdate()
    {
        CheckPlayerVelocity();

        ZoomOut();
        if (Mathf.Abs(myPlayerCurrentVelocity.x) < 2)
        {
            ZoomIn();
        }
        CheckPlayerScreenLocation();
        Move();
        
    }
    public void ResetCameraPosition()
    {
        myPlayerCurrentVelocity = new Vector3(1,0,0);
        myCamera.fieldOfView = myCameraStartFOV;
        transform.position = myCameraStartPosition;
    }
    private void CheckPlayerVelocity()
    {
        myPlayerNewPos = myParentTransform.transform.position;
        myPlayerCurrentVelocity = ((myPlayerNewPos - myPlayerPrevPos) / Time.deltaTime) * 0.5f;
        myPlayerPrevPos = myPlayerNewPos;
    }

    private void Move()
    {
        myPositionToMoveTo = new Vector3(myParentTransform.transform.position.x + (myPlayerCurrentVelocity.x * myHorizontalAcceleration) - myPlayerHorizontalCameraPosition,
                                        myParentTransform.transform.position.y + (myPlayerCurrentVelocity.y * myVerticalAcceleration) - myPlayerVerticalCameraPosition,
                                        transform.position.z);
        if (myPlayerMovement.myIsSliding)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + newFloat, transform.position.z);
        }
        myTargetPosition = transform.position;
        
        
        if (myPlayerScreenPoint.x >= myPixelsAllowedFromLeft && myPlayerScreenPoint.x < Screen.width - myPixelsAllowedFromRight)
        {
            myTargetPosition.x = myPositionToMoveTo.x;
            
            transform.position = Vector3.Lerp(transform.position, myTargetPosition, Time.deltaTime);
        }
        else if (myPlayerScreenPoint.x < myPixelsAllowedFromLeft)
        {
            
            myBoundaryWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(myPixelsAllowedFromLeft, myPlayerScreenPoint.y, myPlayerScreenPoint.z));
            myCenterWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(Screen.width/2, myPlayerScreenPoint.y, myPlayerScreenPoint.z));
            float dif = myCenterWorldPoint.x - myBoundaryWorldPoint.x;
            myTargetPosition.x = myParentTransform.transform.position.x + dif;
            transform.position = myTargetPosition;
        }
        else if (myPlayerScreenPoint.x > Screen.width - myPixelsAllowedFromRight)
        {
            
            myBoundaryWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(Screen.width-myPixelsAllowedFromRight, myPlayerScreenPoint.y, myPlayerScreenPoint.z));
            myCenterWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, myPlayerScreenPoint.y, myPlayerScreenPoint.z));
            float dif = myCenterWorldPoint.x - myBoundaryWorldPoint.x;
            myTargetPosition.x = myParentTransform.transform.position.x + dif;
            transform.position = myTargetPosition;
        }
        if (myPlayerScreenPoint.y >= myPixelsAllowedFromDown && myPlayerScreenPoint.y < Screen.height - myPixelsAllowedFromUp)
        {
            
            myTargetPosition.y = myPositionToMoveTo.y;
           
            transform.position = Vector3.Lerp(transform.position, myTargetPosition, Time.deltaTime);
        }
        else if (myPlayerScreenPoint.y < myPixelsAllowedFromDown)
        {
            
            myBoundaryWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(myPlayerScreenPoint.x, myPixelsAllowedFromDown, myPlayerScreenPoint.z));
            myCenterWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(myPlayerScreenPoint.x, Screen.height/2, myPlayerScreenPoint.z));
            float dif = myCenterWorldPoint.y - myBoundaryWorldPoint.y;
            myTargetPosition.y = myParentTransform.transform.position.y + dif;
            transform.position = myTargetPosition;
        }
        else if (myPlayerScreenPoint.y > Screen.height - myPixelsAllowedFromUp)
        {
          
            myBoundaryWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(myPlayerScreenPoint.x, Screen.height-myPixelsAllowedFromUp, myPlayerScreenPoint.z));
            myCenterWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(myPlayerScreenPoint.x, Screen.height / 2, myPlayerScreenPoint.z));
            float dif = myCenterWorldPoint.y - myBoundaryWorldPoint.y;
            myTargetPosition.y = myParentTransform.transform.position.y + dif;
            transform.position = myTargetPosition;
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

        myPlayerScreenPoint = myCamera.WorldToScreenPoint(myParentTransform.transform.position);

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
