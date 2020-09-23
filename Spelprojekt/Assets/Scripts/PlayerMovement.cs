using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float myMaxSpeed = 10;
    [SerializeField]
    float myAcceleration = 1;
    [SerializeField]
    float myDecceleration = 1;
    [SerializeField]
    float myDrag = 0.1f;
    [SerializeField]
    float myJumpForce = 15;
    [SerializeField]
    float myGravity = 1f;
    float myJumpTimer = 0f;
    float myJumpTime = 0.25f;
    int myInputDirectionX = 0;
    int myInputDirectionY = 0;
    [SerializeField]
    int myXDierction = 0;


    [SerializeField]
    KeyCode myJumpKey = KeyCode.Space;
    [SerializeField]
    KeyCode myMoveLeftKey = KeyCode.A;
    [SerializeField]
    KeyCode myMoveRightKey = KeyCode.D;

    [SerializeField]
    LayerMask layerMask;
    Vector3 myCurrentVelocity;

    bool myIsGrounded;

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
        myIsGrounded = CheckGround();
        GetInputs();
        if (myCurrentVelocity.x > 0)
        {
            myXDierction = 1;
        }
        if (myCurrentVelocity.x < 0)
        {
            myXDierction = -1;
        }
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
        else if (Input.GetKey(myMoveLeftKey))
        {
            myInputDirectionX = -1;
        }
        else if (Input.GetKey(myMoveRightKey))
        {
            myInputDirectionX = 1;
        }
        else
        {
            myInputDirectionX = 0;
        }
        //-------------------
        if (Input.GetKey(myJumpKey) && CheckGround())
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
    bool CheckGround()
    {
        if (Physics2D.BoxCast(transform.position, transform.localScale, 0 , Vector3.down, 0.1f ,layerMask))
        {
            return true;
        }
        else
        {

            return false;
        }

    }
    void CastBox()
    {
        RaycastHit2D[] hitInfo = Physics2D.BoxCastAll(transform.position, transform.localScale, 0, myCurrentVelocity.normalized, myCurrentVelocity.magnitude * Time.fixedDeltaTime, layerMask);

        Vector2 temp = new Vector2();

        foreach (var item in hitInfo)
        {

            if(item.collider != null)
            {

                temp += item.normal;

            }

        }

        print(temp);

        //ApplyForce(-temp * myCurrentVelocity);

        if (temp.y < 0 && myCurrentVelocity.y > 0)
        {



            myCurrentVelocity.y = 0;
            //ApplyForce(myCurrentVelocity.magnitude * temp);
        }
        //if (temp.y > 0 && myCurrentVelocity.y < 0)
        //{

        //    myCurrentVelocity.y = 0;
        //    //ApplyForce(myCurrentVelocity.magnitude * temp);
        //}

        if (temp.x > 0 && myCurrentVelocity.x < 0)
        {

            myCurrentVelocity.x = 0;
            //ApplyForce(myCurrentVelocity.magnitude * temp);

        }
        if (temp.x < 0 && myCurrentVelocity.x > 0)
        {
            myCurrentVelocity.x = 0;

            //ApplyForce(myCurrentVelocity.magnitude * temp);

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
            if (myXDierction == 1)
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
            DoJump();

        }
        if (!myIsGrounded)
        {
            if (myCurrentVelocity.y > 0)
            {
                ApplyForce(new Vector3(0, -myGravity * 3, 0));
            }
            else
            {
                ApplyForce(new Vector3(0, -myGravity / 2, 0));

                Debug.Log("go SLOW down");
            }
        }
        else
        {
            if (myCurrentVelocity.y < 0)
            {
                myCurrentVelocity.y = 0;

            }
        }

        CastBox();

        transform.Translate(myCurrentVelocity * Time.fixedDeltaTime);
    }

    void DoJump()
    {
        ApplyForce(new Vector3(0, myJumpForce, 0));


    }
    void DoSlide()
    {

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position + myCurrentVelocity, transform.localScale);
    }
}
