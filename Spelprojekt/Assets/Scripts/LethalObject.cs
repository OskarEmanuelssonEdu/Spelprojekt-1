using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalObject : MonoBehaviour
{
    [Header("Autofilled variables")]

    [SerializeField]
    [Tooltip("Autofilled")]
    Player myPlayer;
    [SerializeField]
    [Tooltip("Autofilled")]
    PlayerMovement myPlayerMovement;

    [Header("Debug Options")]

    [Tooltip("This will determine the \"name\" of the object when logging messages.")]
    public string myName;
    [SerializeField]
    [Tooltip("Boolean indicating wether a white frame shall be rendered showing the hitbox of the lethal object.")]
    bool myShowHitbox;
    [SerializeField]
    [Tooltip("Boolean indicating wether a frame around the lethal object will be rendered showing where the player \"intersects\" with the lethal object.")]
    bool myShowCollision;
    [SerializeField]
    [Tooltip("Tick to log when the LethalObject collides with the player.")]
    bool myLogCollision;
    bool myHasLoggedCollision;
    bool myHasCollided = false;

    // Used for collision prediction
    Vector3 myPreviousPosition;
    Vector3 myDeltaPosition;

    void OnValidate()
    {
        if (myName == "")
        {
            myName = "Unnamed Lethal Object";
        }

        myPlayer = FindObjectOfType<Player>();
        myPlayerMovement = FindObjectOfType<PlayerMovement>();
    }

    void Start()
    {
        if (myName == "")
        {
            myName = "Unnamed Lethal Object";
        }

        myPlayer = FindObjectOfType<Player>();
        myPlayerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        myDeltaPosition = (myPreviousPosition - transform.position);
        myPreviousPosition = transform.position;

        Vector3 rectangleOneScale = transform.localScale,
                rectangleTwoScale = myPlayerMovement.MyHitbox,
                rectangleOnePosition = transform.position + myDeltaPosition,
                rectangleTwoPosition = myPlayer.transform.position;

        // Calculate the sides of the rectangles in order to detect collision
        float rectangleOneRightSide = rectangleOnePosition.x - rectangleOneScale.x * .5f,
              rectangleOneLeftSide = rectangleOnePosition.x + rectangleOneScale.x * .5f,
              rectangleOneTopSide = rectangleOnePosition.y + rectangleOneScale.y * .5f,
              rectangleOneBottomSide = rectangleOnePosition.y - rectangleOneScale.y * .5f,
              rectangleTwoRightSide = rectangleTwoPosition.x - rectangleTwoScale.x * .5f,
              rectangleTwoLeftSide = rectangleTwoPosition.x + rectangleTwoScale.x * .5f,
              rectangleTwoTopSide = rectangleTwoPosition.y + rectangleTwoScale.y * .5f,
              rectangleTwoBottomSide = rectangleTwoPosition.y - rectangleTwoScale.y * .5f;

        // Collision detection between two rectangles
        if (rectangleOneRightSide < rectangleTwoLeftSide
            && rectangleOneLeftSide > rectangleTwoRightSide
            && rectangleOneBottomSide < rectangleTwoTopSide
            && rectangleOneTopSide > rectangleTwoBottomSide)

        {
            Vector3 cachedPosition = transform.position;
            if (!myHasCollided &&
                !(cachedPosition.x > 2160.0f && cachedPosition.y < -200.0f)    
            )
            {
                myHasCollided = true;
                AudioManager.ourPublicInstance.PlayLethalHit();
                //Debug.Log(string.Format("{0} started intersecting Player at: (X: {1} | Y: {2} | Z: {3})", myName, transform.position.x, transform.position.y, transform.position.z));
            }

            if (myLogCollision && !myHasLoggedCollision)
            {
                
                myHasLoggedCollision = true;
            }
            myPlayer.TakeDamage(myPlayer.myCurrentHealth);
            if (!myHasLoggedCollision)
            {
                

            }

        }
        else
        {
            myHasCollided = false;

            if (myHasLoggedCollision)
            {


                if (myLogCollision && !myHasLoggedCollision)
                {
                    //Debug.Log(string.Format("{0} started intersecting Player at: (X: {1} | Y: {2} | Z: {3})", myName, transform.position.x, transform.position.y, transform.position.z));
                    myHasLoggedCollision = true;
                }
                //myPlayer.TakeDamage(myDamage * Time.deltaTime);
            }
            else if (myHasLoggedCollision)
            {

                //Debug.Log(string.Format("{0} stopped intersecting Player at: (X: {1} | Y: {2} | Z: {3})", myName, transform.position.x, transform.position.y, transform.position.z));
                myHasLoggedCollision = false;
            }
        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        
        if (myShowHitbox && !myShowCollision)
        {
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
        if (myShowCollision)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, transform.position + myDeltaPosition);

            Vector3 rectangleOneScale = transform.localScale,
                    rectangleTwoScale = myPlayer.transform.localScale,
                    rectangleOnePosition = transform.position + myDeltaPosition,
                    rectangleTwoPosition = myPlayer.transform.position;

            // Calculate the sides of the rectangles in order to detect collision
            float rectangleOneRightSide = rectangleOnePosition.x - rectangleOneScale.x * .5f,
                  rectangleOneLeftSide = rectangleOnePosition.x + rectangleOneScale.x * .5f,
                  rectangleOneTopSide = rectangleOnePosition.y + rectangleOneScale.y * .5f,
                  rectangleOneBottomSide = rectangleOnePosition.y - rectangleOneScale.y * .5f,
                  rectangleTwoRightSide = rectangleTwoPosition.x - rectangleTwoScale.x * .5f,
                  rectangleTwoLeftSide = rectangleTwoPosition.x + rectangleTwoScale.x * .5f,
                  rectangleTwoTopSide = rectangleTwoPosition.y + rectangleTwoScale.y * .5f,
                  rectangleTwoBottomSide = rectangleTwoPosition.y - rectangleTwoScale.y * .5f;

            Gizmos.color = Color.white;
            if (rectangleOneRightSide < rectangleTwoLeftSide)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawLine(
                new Vector3(rectangleOneLeftSide, rectangleOneBottomSide, transform.position.z),
                new Vector3(rectangleOneLeftSide, rectangleOneTopSide, transform.position.z));

            Gizmos.color = Color.white;
            if (rectangleOneLeftSide > rectangleTwoRightSide)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawLine(
                new Vector3(rectangleOneRightSide, rectangleOneBottomSide, transform.position.z),
                new Vector3(rectangleOneRightSide, rectangleOneTopSide, transform.position.z));

            Gizmos.color = Color.white;
            if (rectangleOneBottomSide < rectangleTwoTopSide)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawLine(
                new Vector3(rectangleOneRightSide, rectangleOneTopSide, transform.position.z),
                new Vector3(rectangleOneLeftSide, rectangleOneTopSide, transform.position.z));

            Gizmos.color = Color.white;
            if (rectangleOneTopSide > rectangleTwoBottomSide)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawLine(
                new Vector3(rectangleOneRightSide, rectangleOneBottomSide, transform.position.z),
                new Vector3(rectangleOneLeftSide, rectangleOneBottomSide, transform.position.z));
        }
    }
}