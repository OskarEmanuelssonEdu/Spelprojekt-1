using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Speed settings")]


    [SerializeField]
    [Range(0, 50)]
    float myMaxSpeed = 10;
    [SerializeField]
    [Range(0.1f, 50)]
    float myAcceleration = 1;
    [SerializeField]
    [Range(0.1f, 50)]
    float myDecceleration = 1;
    [SerializeField]
    [Range(0.1f, 1)]
    float myDrag = 0.1f;

    [Header("Jump settings")]

    [SerializeField]
    [Range(0, 100)]
    float myJumpForce = 15;
    [SerializeField]
    [Range(1, 100)]
    float myGravity = 1f;
    [SerializeField]
    [Range(0, 2)]
    float myJumpTime = 0.25f;
    float myJumpTimer = 0f;
    [SerializeField]
    [Range(0, 100)]
    float myJumpStartForce = 1f;


    int myInputDirectionX = 0;
    int myInputDirectionY = 0;
    int myXDierction = 0;


    Vector3 myColliderSize = new Vector3(1, 1, 1);
    Vector3 myCurrentColliderSize = new Vector3(1, 1, 1);


    [Header("Input Settings")]
    [SerializeField]
    KeyCode myJumpKey = KeyCode.Space;
    [SerializeField]
    KeyCode myMoveLeftKey = KeyCode.A;
    [SerializeField]
    KeyCode myMoveRightKey = KeyCode.D;
    [SerializeField]
    KeyCode mySlideKey = KeyCode.LeftShift;


    [Header("DO NOT TOUCH")]
    [SerializeField]
    LayerMask myLayerMask;
    bool myIsGrounded;
    bool myIsSliding;
    Vector3 myCurrentVelocity;

    public Vector3 MyCurrentVelocity { get { return myCurrentVelocity;  } set { myCurrentVelocity = value; } }

    JumpState myJumpState;

    public Vector3 CurrentSpeed
    {
        get
        {
            return myCurrentVelocity;
        }
        set
        {
            myCurrentVelocity = value;
        }
    }
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

<<<<<<< HEAD

=======
>>>>>>> 7a2144a16494d1d5df51e0d2e443fbe7d9f704d0
    }
    void FixedUpdate()
    {

        DoPhysics();
    }
    public void ApplyForce(Vector3 aTargetVelocity)
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
        if (Input.GetKey(myJumpKey))
        {
            myInputDirectionY = 1;
        }
        else
        {
            myInputDirectionY = 0;
        }

        if (Input.GetKeyDown(mySlideKey))
        {

            DoEnterSlide();

        }
        else if (Input.GetKeyUp(mySlideKey))
        {

            DoExitSlide();

        }

    }
    void Deccelerate()
    {
        myCurrentVelocity.x = Mathf.MoveTowards(myCurrentVelocity.x, 0, myDecceleration);
    }
    bool CheckGround()
    {
        if (Physics2D.BoxCast(transform.position, new Vector3(transform.localScale.x * 0.9f, transform.localScale.y * 1, transform.localScale.z * 0.9f), 0, Vector3.down, 0.1f, myLayerMask))
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
        RaycastHit2D[] hitInfo = Physics2D.BoxCastAll(transform.position, myCurrentColliderSize, 0, myCurrentVelocity.normalized, myCurrentVelocity.magnitude * Time.fixedDeltaTime, myLayerMask);

        Vector2 hitNormals = new Vector2();

        foreach (var item in hitInfo)
        {

            if (item.collider != null)
            {

                hitNormals += item.normal;

            }

        }

        //ApplyForce(-temp * myCurrentVelocity);

        if (hitNormals.y < 0 && myCurrentVelocity.y > 0)
        {

            myCurrentVelocity.y = 0;
            for (int i = 0; i < hitInfo.Length; i++)
            {
                transform.position = hitInfo[i].centroid;
            }


            //ApplyForce(myCurrentVelocity.magnitude * temp);
        }
        if (hitNormals.y > 0 && myCurrentVelocity.y < 0)
        {


            if (hitNormals.y < 1 && hitNormals.y > 0 && myIsSliding)
            {

                DoSlideDownSlope(hitNormals);


            }
            else
            {

                myCurrentVelocity.y = 0;
                for (int i = 0; i < hitInfo.Length; i++)
                {
                    transform.position = hitInfo[i].centroid;
                }

            }

            //ApplyForce(myCurrentVelocity.magnitude * temp);
        }
        if (hitNormals.x > 0 && myCurrentVelocity.x < 0) //going left
        {

            if (hitNormals.x > 0 && hitNormals.x < 0.6)
            {


                DoMoveAlongSlope(hitNormals);


            }

            else
            {

                myCurrentVelocity.x = 0;
                for (int i = 0; i < hitInfo.Length; i++)
                {
                    transform.position = hitInfo[i].centroid;
                }

                //ApplyForce(myCurrentVelocity.magnitude * temp);
            }


        }
        if (hitNormals.x < 0 && myCurrentVelocity.x > 0) //going right
        {

            if (hitNormals.x < 0 && hitNormals.x > -0.6)
            {


                DoMoveAlongSlope(hitNormals);

            }
            else
            {

                myCurrentVelocity.x = 0;
                for (int i = 0; i < hitInfo.Length; i++)
                {
                    transform.position = hitInfo[i].centroid;
                }

                //ApplyForce(myCurrentVelocity.magnitude * temp);
            }




        }



    }
    void DoPhysics()
    {

        if (myCurrentVelocity.magnitude < myMaxSpeed && !myIsSliding)
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
        if (myInputDirectionX == 0 && !myIsSliding && myIsGrounded)
        {
            Deccelerate();
        }

        switch (myJumpState)
        {
            case JumpState.none:

                ApplyForce(new Vector3(0, -myGravity, 0));

                if (myIsGrounded && myInputDirectionY == 1)
                {

                    DoExitSlide();
                    myCurrentVelocity.y = 0;
                    myJumpTimer = 0;
                    ApplyForce(new Vector3(0, myJumpStartForce, 0));
                    myJumpState = JumpState.jumping;


                }
                else if (!myIsGrounded)
                {

                    myJumpState = JumpState.falling;

                }

                break;

            case JumpState.jumping:


                if (myInputDirectionY < 1 || myJumpTimer > myJumpTime)
                {

                    myJumpState = JumpState.falling;

                }
                ApplyForce(new Vector3(0, myJumpForce * Time.deltaTime, 0));

                myJumpTimer += Time.fixedDeltaTime;


                break;
            case JumpState.falling:

                ApplyForce(new Vector3(0, -myGravity, 0));

                if (myIsGrounded)
                {

                    myJumpState = JumpState.none;


                }

                break;

        }

        CastBox();

        transform.Translate(myCurrentVelocity * Time.fixedDeltaTime);
    }
    void DoJump()
    {

        myCurrentVelocity.y = 0;
        ApplyForce(new Vector3(0, myJumpForce, 0));


    }
    void DoEnterSlide()
    {
        myIsSliding = true;
        myCurrentColliderSize = new Vector3(myColliderSize.x, 0.5f, myColliderSize.z);

    }
    void DoExitSlide()
    {

        transform.Translate(new Vector3(0, myColliderSize.y - myCurrentColliderSize.y, 0));

        myIsSliding = false;
        myCurrentColliderSize = myColliderSize;


    }
    void DoMoveAlongSlope(Vector3 someNormals)
    {

        Vector3 positiveNormal = Quaternion.Euler(0, 0, 90) * someNormals;
        Vector3 negativeNormal = Quaternion.Euler(0, 0, -90) * someNormals;

        float normalSignedAngle = Vector3.SignedAngle(someNormals, myCurrentVelocity, Vector3.forward);

        // print(normalSignedAngle);

        if (normalSignedAngle > 0)
        {


            myCurrentVelocity = Vector3.Project(myCurrentVelocity, negativeNormal);

        }
        else if (normalSignedAngle < 0)
        {

            myCurrentVelocity = Vector3.Project(myCurrentVelocity, positiveNormal);


        }

        Debug.DrawRay(transform.position, someNormals, Color.red);

    }
    void DoSlideDownSlope(Vector3 someNormals)
    {

        Vector3 positiveNormal = Quaternion.Euler(0, 0, 90) * someNormals;
        Vector3 negativeNormal = Quaternion.Euler(0, 0, -90) * someNormals;

        float normalSignedAngle = Vector3.SignedAngle(someNormals, myCurrentVelocity, Vector3.forward);

        // print(normalSignedAngle);

        if (normalSignedAngle > 0)
        {


            myCurrentVelocity = Vector3.Project(myCurrentVelocity, positiveNormal);
            Debug.DrawRay(transform.position, Vector3.Project(myCurrentVelocity, negativeNormal), Color.red);

        }
        else if (normalSignedAngle < 0)
        {

            myCurrentVelocity = Vector3.Project(myCurrentVelocity, negativeNormal);

            Debug.DrawRay(transform.position, Vector3.Project(myCurrentVelocity, positiveNormal), Color.red);

        }

        Debug.DrawRay(transform.position, someNormals, Color.red);


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + myCurrentVelocity * Time.fixedDeltaTime, myCurrentColliderSize);
    }
}
