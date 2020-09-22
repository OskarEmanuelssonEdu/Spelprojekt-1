using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    float myMaxSpeed = 10;
    float myAcceleration = 1;

    int myInputDirectionX = 0;
    int myInputDirectionY = 0;

    [SerializeField]
    KeyCode myJumpKey = KeyCode.Space;
    [SerializeField]
    KeyCode myMoveLeftKey = KeyCode.A;
    [SerializeField]
    KeyCode myMoveRightKey = KeyCode.D;

    Vector3 myCurrentVelocity;
    
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        
    }
    void ApplyForce(Vector3 aTargetVelocity)
    {

        
    }
    void GetInputs()
    {

    }
    void CastBox()
    {

    }
    void PredictBox()
    {

    }
    void DoPhysics()
    {

    }
    void DoSlide()
    {

    }
}
