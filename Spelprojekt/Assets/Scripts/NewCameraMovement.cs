using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraMovement : MonoBehaviour
{

    
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
    [Range(0f, 1f)]
    private float myVerticalAcceleration = 0.3f;
    [SerializeField]
    [Range(0f, 1f)]
    private float myHorizontalAcceleration = 0.2f;


    public Vector3 myPlayerCurrentVelocity;
    Vector3 myPlayerPrevPos;
    Vector3 myPlayerNewPos;

    Vector3 myPositionToMoveTo;
    Vector3 myPlayerScreenPoint;
    Vector3 myTargetPosition;
    Vector3 myBoundaryWorldPoint;
    Vector3 myCenterWorldPoint;

    Vector3 myCameraStartPosition;
    float myCameraStartFOV;

   

    Vector3 zeroVector;
    private void Start()
    {
        if (myCamera == null)
        {
            myCamera = Camera.main;
        }
        myPlayerPrevPos = myPlayer.transform.position;
        myCameraStartPosition = myCamera.transform.localPosition;
        myCameraStartFOV = myCamera.fieldOfView;
        zeroVector = Vector3.zero;
    }
    private void OnValidate()
    {
        myPlayer = FindObjectOfType<Player>();
        myPlayerMovement = FindObjectOfType<PlayerMovement>();
    }
    void FixedUpdate()
    {
        CheckPlayerScreenLocation();
        CheckPlayerVelocity();
        Move();

        ZoomOut();
        if (Mathf.Abs(myPlayerCurrentVelocity.x) < 2)
        {
            ZoomIn();
        }

    }
    private void LateUpdate()
    {
        
    }

    public void ResetCameraPosition()
    {
        Debug.Log("Camera Reset");
        myPlayerCurrentVelocity = Vector3.zero;
        myCamera.fieldOfView = myCameraStartFOV;
        //myCamera.transform.localPosition = new Vector3(0, 0, myCameraStartPosition.z);
        //transform.position = new Vector3(myPlayer.transform.position.x, myPlayer.transform.position.y, myCameraStartPosition.z);
       
    }
    private void CheckPlayerVelocity()
    {
        myPlayerCurrentVelocity = myPlayerMovement.CurrentSpeed;
        //myPlayerNewPos = myPlayer.transform.position;
        //myPlayerCurrentVelocity = ((myPlayerNewPos - myPlayerPrevPos) / Time.fixedDeltaTime) ;
        //myPlayerPrevPos = myPlayerNewPos;
    }

    private void Move()
    {
        
        myPositionToMoveTo = new Vector3(myPlayer.transform.position.x + (myPlayerCurrentVelocity.x * myHorizontalAcceleration) - myPlayerHorizontalCameraPosition,
                                        myPlayer.transform.position.y + (myPlayerCurrentVelocity.y * myVerticalAcceleration) - myPlayerVerticalCameraPosition,
                                        transform.position.z);
        
        myTargetPosition = transform.position;
        
        
        if (myPlayerScreenPoint.x >= myPixelsAllowedFromLeft && myPlayerScreenPoint.x < Screen.width - myPixelsAllowedFromRight)
        {
           
                myTargetPosition.x = myPositionToMoveTo.x;

            

            //transform.position = Vector3.Lerp(transform.position, myTargetPosition, Time.fixedDeltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, myTargetPosition, Time.fixedDeltaTime);

            transform.position = Vector3.SmoothDamp(transform.position, myTargetPosition, ref zeroVector, 3f);
        }
        else if (myPlayerScreenPoint.x < myPixelsAllowedFromLeft)
        {
            myBoundaryWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(myPixelsAllowedFromLeft, myPlayerScreenPoint.y, myPlayerScreenPoint.z));
            myCenterWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(Screen.width/2, myPlayerScreenPoint.y, myPlayerScreenPoint.z));
            float dif = myCenterWorldPoint.x - myBoundaryWorldPoint.x;
            myTargetPosition.x = myPlayer.transform.position.x + dif-0.01f;
            transform.position = myTargetPosition;
        }
        else if (myPlayerScreenPoint.x > Screen.width - myPixelsAllowedFromRight)
        {
            myBoundaryWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(Screen.width-myPixelsAllowedFromRight, myPlayerScreenPoint.y, myPlayerScreenPoint.z));
            myCenterWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, myPlayerScreenPoint.y, myPlayerScreenPoint.z));
            float dif = myCenterWorldPoint.x - myBoundaryWorldPoint.x;
            myTargetPosition.x = myBoundaryWorldPoint.x + dif + 0.01f;
            transform.position = myTargetPosition;
        }
        //transform.position = myPositionToMoveTo;
        if (myPlayerScreenPoint.y >= myPixelsAllowedFromDown && myPlayerScreenPoint.y < Screen.height - myPixelsAllowedFromUp)
        {
            myTargetPosition.y = myPositionToMoveTo.y;

            //transform.position = Vector3.Lerp(transform.position, myTargetPosition, Time.fixedDeltaTime);
            transform.position = Vector3.SmoothDamp(transform.position, myTargetPosition, ref zeroVector, 3f);
        }
        else if (myPlayerScreenPoint.y < myPixelsAllowedFromDown)
        {
            myBoundaryWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(myPlayerScreenPoint.x, myPixelsAllowedFromDown, myPlayerScreenPoint.z));
            myCenterWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(myPlayerScreenPoint.x, Screen.height / 2, myPlayerScreenPoint.z));
            float dif = myCenterWorldPoint.y - myBoundaryWorldPoint.y;
            myTargetPosition.y = myPlayer.transform.position.y + dif- 0.01f;
            transform.position = myTargetPosition;
        }
        else if (myPlayerScreenPoint.y > Screen.height - myPixelsAllowedFromUp)
        {
            myBoundaryWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(myPlayerScreenPoint.x, Screen.height - myPixelsAllowedFromUp, myPlayerScreenPoint.z));
            myCenterWorldPoint = myCamera.ScreenToWorldPoint(new Vector3(myPlayerScreenPoint.x, Screen.height / 2, myPlayerScreenPoint.z));
            float dif = myCenterWorldPoint.y - myBoundaryWorldPoint.y;
            myTargetPosition.y = myPlayer.transform.position.y + dif +0.01f;
            transform.position = myTargetPosition;
        }
    }
    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        

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
    }
    private void ZoomIn()
    {
        myCamera.fieldOfView -= myZoomInSpeed * Time.fixedDeltaTime;
        if (myCamera.fieldOfView < myMinFieldOfView)
        {
            myCamera.fieldOfView = myMinFieldOfView;
        }
    }
    private void ZoomOut()
    {
        myCamera.fieldOfView += Mathf.Abs(myPlayerCurrentVelocity.x) * myZoomOutSpeed * Time.fixedDeltaTime;
        if (myCamera.fieldOfView > myMaxFieldOfView)
        {
            myCamera.fieldOfView = myMaxFieldOfView;
        }
    }

}
