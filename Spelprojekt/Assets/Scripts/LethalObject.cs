using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalObject : MonoBehaviour
{
    public float myDamage;

  
    Player myPlayer;

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

 HEAD


    void OnValidate()
    {
        myPlayer = FindObjectOfType<Player>();
    }
master
    private void Start()
    {
        if (myName == "")
        {
            myName = "Unnamed Lethal Object";
        }
    }

    private void Update()
    {
        Vector3 rectangleOneScale = transform.localScale,
                rectangleTwoScale = myPlayer.transform.localScale,
                rectangleOnePosition = transform.position,
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
HEAD
        {
            if (myLogCollision && !myHasLoggedCollision)
            {
                Debug.Log(string.Format("{0} started intersecting Player at: (X: {1} | Y: {2} | Z: {3})", myName, transform.position.x, transform.position.y, transform.position.z));
                myHasLoggedCollision = true;
            }
            //myPlayer.TakeDamage(myDamage * Time.deltaTime);
        }
        else if (myHasLoggedCollision)
        {

        {
            if (myLogCollision && !myHasLoggedCollision)
            {
                Debug.Log(string.Format("{0} started intersecting Player at: (X: {1} | Y: {2} | Z: {3})", myName, transform.position.x, transform.position.y, transform.position.z));
                myHasLoggedCollision = true;
            }
            //myPlayer.TakeDamage(myDamage * Time.deltaTime);
        }
        else if (myHasLoggedCollision)
        {
master
            Debug.Log(string.Format("{0} stopped intersecting Player at: (X: {1} | Y: {2} | Z: {3})", myName, transform.position.x, transform.position.y, transform.position.z));
            myHasLoggedCollision = false;
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
            Vector3 rectangleOneScale = transform.localScale,
                rectangleTwoScale = myPlayer.transform.localScale,
                rectangleOnePosition = transform.position,
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
                new Vector3(rectangleOneLeftSide, transform.position.y - transform.localScale.y * .5f, transform.position.z),
                new Vector3(rectangleOneLeftSide, transform.position.y + transform.localScale.y * .5f, transform.position.z));

            Gizmos.color = Color.white;
            if (rectangleOneLeftSide > rectangleTwoRightSide)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawLine(
                new Vector3(rectangleOneRightSide, transform.position.y - transform.localScale.y * .5f, transform.position.z),
                new Vector3(rectangleOneRightSide, transform.position.y + transform.localScale.y * .5f, transform.position.z));

            Gizmos.color = Color.white;
            if (rectangleOneBottomSide < rectangleTwoTopSide)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawLine(
                new Vector3(transform.position.x - transform.localScale.x * .5f, rectangleOneTopSide, transform.position.z),
                new Vector3(transform.position.x + transform.localScale.x * .5f, rectangleOneTopSide, transform.position.z));

            Gizmos.color = Color.white;
            if (rectangleOneTopSide > rectangleTwoBottomSide)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawLine(
                new Vector3(transform.position.x - transform.localScale.x * .5f, rectangleOneBottomSide, transform.position.z),
                new Vector3(transform.position.x + transform.localScale.x * .5f, rectangleOneBottomSide, transform.position.z));
        }
    }
}