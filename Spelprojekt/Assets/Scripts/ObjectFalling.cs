using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFalling : MonoBehaviour
{
    [SerializeField]
    private AudioClip myFallingSound;
    [SerializeField]
    [Range(0f, 1f)]
    private float myFallingVolume;

    private bool isGrounded;
    Vector3 velocity = new Vector3(0, 0, 0);

    [Tooltip("Adjusts the falling speed/gravity of the falling object")]
    [SerializeField]
    public float gravity;

    [Tooltip("Distance from falling object to player to trigger fall")]
    [SerializeField]
    float myDistanceToActivate;

    [SerializeField]
    [Tooltip("Assigned in script under OnValidate")]
    GameManager myGameManager;

    // Private variables
    Vector3 myStartPosition;
    Quaternion myStartRotation;
    Vector3 myStartScale;


    private void OnValidate()
    {
        myGameManager = FindObjectOfType<GameManager>();
        gameObject.tag = "FallingObject";
    }

    private void Start()
    {
        velocity = Vector3.zero;
        myStartPosition = transform.position;
        myStartRotation = transform.rotation;
        myStartScale = transform.localScale;
    }

    public void ResetMe()
    {
        velocity = Vector3.zero;
        transform.position = myStartPosition;
        transform.rotation = myStartRotation;
        transform.localScale = myStartScale;
        myRunFalling = false;
    }
    bool myRunFalling = false; 
    void FixedUpdate()
    {
        if (CheckPlayerDistance())
        {
            if (!myRunFalling)
            {
                AudioManager.ourPublicInstance.PlayFallingObject(); ;
            }
            myRunFalling = true;

        }
        if (myRunFalling)
        {
            CheckGrounded();
            if (!isGrounded)
            {
                velocity.y -= gravity * Time.fixedDeltaTime;
                //Debug.Log("Current velocity Y = " + velocity.y);
                transform.position += velocity * Time.fixedDeltaTime;
            }

        }
    }
    bool CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, myGameManager.PlayerPosition());
        if (distanceToPlayer < myDistanceToActivate)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void CheckGrounded()
    {
        RaycastHit2D boxResult = Physics2D.BoxCast(transform.position, new Vector3(1, 1f), 0f, Vector3.down, velocity.y * Time.fixedDeltaTime);
        if (boxResult.collider != null)
        {
            myRunFalling = false;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
