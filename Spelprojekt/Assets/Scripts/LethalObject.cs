using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalObject : MonoBehaviour
{
    public float myDamage;

    [SerializeField]
    Player myPlayer;

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
        {
            if (myLogCollision && myHasLoggedCollision)
            {
                Debug.Log(string.Format("Started intersecting Player at: (X: {0} | Y: {1} | Z: {2})", transform.position.x, transform.position.y, transform.position.z));
            }
            myPlayer.TakeDamage(myDamage * Time.deltaTime);
        }
        else if (myHasLoggedCollision)
        {
            Debug.Log(string.Format("Stopped intersecting Player at: (X: {0} | Y: {1} | Z: {2})", transform.position.x, transform.position.y, transform.position.z));
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