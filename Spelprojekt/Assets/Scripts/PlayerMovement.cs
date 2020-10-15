using System.Collections;
using System.Collections.Generic;
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
    [Range(0.01f, 10)]
    float myFriction;
    [SerializeField]
    [Range(0.0f, 1)]
    float mySlidingFrictionFraction;
    [SerializeField]
    [Range(0.1f, 1)]
    float myAirControlFraction;
    [SerializeField]
    [Range(0.1f, 1)]
    float mySlideControlFraction;
    [SerializeField]
    [Range(0.1f, 20)]
    float mySlideDownwardsSpeed;
    float myCurrentControlFraction;

    [Header("Jump settings")]

    [SerializeField]
    [Range(0, 100)]
    float myJumpForce = 15;
    [SerializeField]
    [Range(0, 10)]
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
    int myXDirection = 0;


    Vector3 myColliderSize = new Vector3(1, 2, 1);
    Vector3 myCurrentColliderSize;
    Vector3 myCurrentColliderPosition;


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
    public bool myIsSliding;
    Vector3 myCurrentVelocity;
    JumpState myJumpState;
    [SerializeField]
    Animator animator;
    Transform modelTransform;
    bool walkingUpSlope = false;

    [SerializeField]
    Transform myCameraTransform;

    [SerializeField]
    float myTurnSpeed;
    private void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
        modelTransform = animator.transform;
        myCameraTransform = FindObjectOfType<NewCameraMovement>().transform;
    }
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

    private void Awake()
    {
        myXDirection = 1;
        myCurrentColliderSize = myColliderSize;

    }
    void Update()
    {


        Animate();
        myIsGrounded = CheckGround();
        GetInputs();

        if (myCurrentVelocity.x > 0)
        {
            myXDirection = 1;
        }
        if (myCurrentVelocity.x < 0)
        {
            myXDirection = -1;
        }



    }
    void FixedUpdate()
    {

        modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, Quaternion.Euler(new Vector3(modelTransform.rotation.x, 90 * myXDirection, modelTransform.rotation.z)), myTurnSpeed);


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
        if (Physics2D.BoxCast(transform.position, new Vector3(transform.localScale.x * 0.9f, transform.localScale.y * 0.9f, transform.localScale.z * 0.9f), 0, Vector3.down, 0.7f, myLayerMask))
        {
            animator.SetTrigger("LandTrigger");

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


            if (hitNormals.y < 1 && hitNormals.y > 0 && myIsSliding && myIsGrounded)
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

            if (hitNormals.x >= 0 && hitNormals.x < 0.6f)
            {

                walkingUpSlope = true;
                DoMoveAlongSlope(hitNormals);


            }

            else
            {
                walkingUpSlope = false;        

                if (hitInfo.Length > 1 && Mathf.Abs(Mathf.Abs(hitInfo[0].collider.bounds.extents.y + hitInfo[0].transform.position.y) - Mathf.Abs(hitInfo[1].collider.bounds.extents.y + hitInfo[1].transform.position.y)) < 0.2f)
                {
                    print(Mathf.Abs(hitInfo[1].collider.bounds.extents.y + hitInfo[1].transform.position.y) - Mathf.Abs(hitInfo[0].collider.bounds.extents.y + hitInfo[0].transform.position.y));
                    transform.position += new Vector3(0, Mathf.Abs(hitInfo[1].collider.bounds.extents.y + hitInfo[1].transform.position.y) - Mathf.Abs(hitInfo[0].collider.bounds.extents.y + hitInfo[0].transform.position.y), 0);
                }
                else
                {
                    myCurrentVelocity.x = 0;
                    for (int i = 0; i < hitInfo.Length; i++)
                    {
                        transform.position = hitInfo[i].centroid;
                    }

                }


                //ApplyForce(new Vector3(0, 0.2f, 0));
            }


        }
        if (hitNormals.x < 0 && myCurrentVelocity.x > 0) //going right
        {


            if (hitNormals.x < 0 && hitNormals.x > -0.6 && myIsGrounded)
            {

                walkingUpSlope = true;
                DoMoveAlongSlope(hitNormals);

            }
            else
            {

                walkingUpSlope = false;


                if (hitInfo.Length > 1 && Mathf.Abs(Mathf.Abs(hitInfo[1].collider.bounds.extents.y + hitInfo[1].transform.position.y) - Mathf.Abs(hitInfo[0].collider.bounds.extents.y + hitInfo[0].transform.position.y)) < 0.2f)
                {
                    transform.position += new Vector3(0, Mathf.Abs(hitInfo[1].collider.bounds.extents.y + hitInfo[1].transform.position.y) - Mathf.Abs(hitInfo[0].collider.bounds.extents.y + hitInfo[0].transform.position.y), 0);
                }
                else
                {

                    myCurrentVelocity.x = 0;
                    for (int i = 0; i < hitInfo.Length; i++)
                    {
                        transform.position = hitInfo[i].centroid;
                    }

                }
            }
        }
    }
    void DoPhysics()
    {

        if (myIsGrounded)
        {
            animator.SetBool("isGrounded", true);

            if (myIsSliding)
            {

                myCurrentControlFraction = mySlideControlFraction;

            }
            else
            {

                mySlideControlFraction = 1;

            }

        }
        else
        {
            animator.SetBool("isGrounded", false);
            myCurrentControlFraction = myAirControlFraction;

        }

        if (myCurrentVelocity.magnitude < myMaxSpeed && !myIsSliding)
        {
            ApplyForce(new Vector3(myAcceleration * myInputDirectionX, 0, 0));
        }

        if (myInputDirectionX == 0 && myIsGrounded && !myIsSliding)
        {

            Deccelerate();

        }
        //if (myInputDirectionX == 0 && !myIsSliding)
        //{
        //    Deccelerate();
        //}

        if (myIsGrounded)
        {
            if (myIsSliding)
            {
                ApplyForce((myCurrentVelocity * -1) * (mySlidingFrictionFraction * myFriction) * Time.fixedDeltaTime);

            }
            else
            {

                ApplyForce((myCurrentVelocity * -1) * myFriction * Time.fixedDeltaTime);

            }


        }

        switch (myJumpState)
        {
            case JumpState.none:

                if (myIsSliding)
                {

                    ApplyForce(new Vector3(0, -myGravity, 0));
                }

                if (myIsGrounded && myInputDirectionY == 1)
                {


                    myCurrentVelocity.y = 0;
                    myJumpTimer = 0;
                    ApplyForce(new Vector3(0, myJumpStartForce, 0));
                    animator.SetTrigger("JumpTrigger");
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

                animator.SetTrigger("ExtendedJump");
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

    void DoEnterSlide()
        {


            myIsSliding = true;
            myCurrentColliderSize = new Vector3(myColliderSize.x, myColliderSize.y / 4, myColliderSize.z);


            transform.position = new Vector3(modelTransform.position.x, transform.position.y - 0.75f, modelTransform.position.z);
            modelTransform.localPosition = new Vector3(modelTransform.localPosition.x, modelTransform.localPosition.y + 0.75f, modelTransform.localPosition.z);

            animator.SetBool("SlideBool", true);

            myCameraTransform.localPosition = new Vector3(myCameraTransform.transform.localPosition.x, myCameraTransform.transform.localPosition.y + 0.75f, myCameraTransform.transform.localPosition.z);


        }
        void DoExitSlide()
        {
            modelTransform.localPosition = new Vector3(modelTransform.localPosition.x, modelTransform.localPosition.y - 0.75f, modelTransform.localPosition.z);

            // transform.position = new Vector3(transform.position.x, transform.position.y + (myColliderSize.y - myCurrentColliderSize.y), transform.position.z);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z);
            myIsSliding = false;
            myCurrentColliderSize = myColliderSize;
            myCameraTransform.localPosition = new Vector3(myCameraTransform.transform.localPosition.x, myCameraTransform.transform.localPosition.y - 0.75f, myCameraTransform.transform.localPosition.z);

            animator.SetBool("SlideBool", false);
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



            if (normalSignedAngle > 0)
            {


                myCurrentVelocity = Vector3.Project(myCurrentVelocity, positiveNormal);
                Debug.DrawRay(transform.position, Vector3.Project(myCurrentVelocity, negativeNormal), Color.red);

                modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, Quaternion.FromToRotation(transform.up, someNormals) * Quaternion.Euler(0, -90, 0), myTurnSpeed);




            }
            else if (normalSignedAngle < 0)
            {

                myCurrentVelocity = Vector3.Project(myCurrentVelocity, negativeNormal);

                Debug.DrawRay(transform.position, Vector3.Project(myCurrentVelocity, positiveNormal), Color.red);


                modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, Quaternion.FromToRotation(transform.up, someNormals) * Quaternion.Euler(0, 90, 0), myTurnSpeed);



            }

            print(someNormals);


            Debug.DrawRay(transform.position, someNormals, Color.red);


        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(transform.position, myCurrentColliderSize);
        }
        void Animate()
        {
            animator.SetFloat("isRunning", Mathf.Abs(myCurrentVelocity.x));
        }
    }
