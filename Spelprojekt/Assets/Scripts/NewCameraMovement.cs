using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraMovement : MonoBehaviour
{
    //[SerializeField]
    //private AudioClip myAudioWhenNearScreen;
    //[SerializeField]
    //private AudioClip myAudioDeath;

    [SerializeField]
    Camera myCamera;
    [SerializeField]
    private GameObject myPlayer;

    



    [Header("BOUNDARY Settings")]
    [SerializeField]
    [Range(-30f, 30f)]
    private float myPlayerHorizontalCameraPosition;
    [SerializeField]
    [Range(-20f, 20f)]
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

    Vector3 myPlayerCurrentVelocity;
    Vector3 myPlayerPrevPos;
    Vector3 myPlayerNewPos;
    Vector3 myDistanceBetweenCameraAndPlayer;
    
    Vector3 positionToMoveTo;

    private void Start()
    {
       
        myPlayerPrevPos = myPlayer.transform.position;
      
    }
    void FixedUpdate()
    {
        CheckPlayerVelocity();
        Debug.Log("Player velocity: " + myPlayerCurrentVelocity);

        ZoomOut();
        if (Mathf.Abs(myPlayerCurrentVelocity.x) < 1)
        {
            ZoomIn();
        }
        Move();
        CheckPlayerScreenLocation();
    }

    private void CheckPlayerVelocity()
    {
        myPlayerNewPos = myPlayer.transform.position;
        myPlayerCurrentVelocity = ((myPlayerNewPos - myPlayerPrevPos) / Time.deltaTime) * 0.5f;
        myPlayerPrevPos = myPlayerNewPos;
    }

    private void Move()
    {
        positionToMoveTo = new Vector3(myPlayer.transform.position.x + myPlayerCurrentVelocity.x-myPlayerHorizontalCameraPosition, 
                                        myPlayer.transform.position.y + myPlayerCurrentVelocity.y+myPlayerVerticalCameraPosition, 
                                        transform.position.z);
        transform.position =  Vector3.Lerp(transform.position,positionToMoveTo,Time.deltaTime);
    }













    private void CheckPlayerScreenLocation()
    {
        Vector3 screenPoint = myCamera.WorldToScreenPoint(myPlayer.transform.position);

        
       

        
        
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
