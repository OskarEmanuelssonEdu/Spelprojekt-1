using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    float myMaxSpeed = 10;
    float myAcceleration = 1;
    float myDecceleration = 1;
    float myDrag = 0.1f;
    int myInputDirectionX = 0;
    int myInputDirectionY = 0;

    [SerializeField]
    KeyCode myJumpKey = KeyCode.Space;
    [SerializeField]
    KeyCode myMoveLeftKey = KeyCode.A;
    [SerializeField]
    KeyCode myMoveRightKey = KeyCode.D;

    [SerializeField]
    LayerMask layerMask;
    Vector3 myCurrentVelocity;

    JumpState myJumpState;
    enum JumpState
    {
        none,
        jumping,
        falling
    }
    void Start()
    {
        
    }
    void Update()
    {
        GetInputs();
    }
    void FixedUpdate()
    {

        DoPhysics();
    }
    void ApplyForce(Vector3 aTargetVelocity)
    {
        myCurrentVelocity = myCurrentVelocity + aTargetVelocity;
        
    }
    void GetInputs()
    {
        if (Input.GetKey(myMoveLeftKey) && Input.GetKey(myMoveRightKey))
        {
            myInputDirectionX = 0;
        }
        else if(Input.GetKey(myMoveLeftKey))
        {
            myInputDirectionX = -1;
        }
        else if(Input.GetKey(myMoveRightKey))
        {
            myInputDirectionX = 1;
        }
        else
        {
            myInputDirectionX = 0;
        }
        //-------------------
        if (Input.GetKey(myJumpKey))
        {
            myInputDirectionY = 1;
        }
        else
        {
            myInputDirectionY = 0;
        }

    }
    void Deccelerate()
    {
        myCurrentVelocity.x = Mathf.MoveTowards(myCurrentVelocity.x, 0, myDecceleration);
    }
    void CastBox()
    {
        RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, transform.localScale , 0, (myCurrentVelocity * Time.fixedDeltaTime) - (transform.localScale / 2) * myInputDirectionX, layerMask);
        
        if (hitInfo.collider != null)
        {
            if (hitInfo.normal.x > 0 && myInputDirectionX == -1)
            {
                 ApplyForce(myCurrentVelocity.magnitude * new Vector3(hitInfo.normal.x, hitInfo.normal.y,0));

            }
            if (hitInfo.normal.x < 0 && myInputDirectionX == 1)
            {
                ApplyForce(myCurrentVelocity.magnitude * new Vector3(hitInfo.normal.x, hitInfo.normal.y, 0));

            }

        }

    }
    void DoPhysics()
    {

        if (myCurrentVelocity.magnitude < myMaxSpeed)
        {
            ApplyForce(new Vector3(myAcceleration * myInputDirectionX, 0, 0));
        }
        else
        {
            if (myCurrentVelocity.x > 0)
            {
                myCurrentVelocity.x -= myDrag;
            }
            else
            {
                myCurrentVelocity.x += myDrag;
            }
        }
        if (myInputDirectionX == 0)
        {
            Deccelerate();
        }
        if (myInputDirectionY == 1)
        {

        }

        CastBox();

        transform.Translate(myCurrentVelocity * Time.fixedDeltaTime);
    }
    
    void DoJump()
    {

    }
    void DoSlide()
    {

    }
}
