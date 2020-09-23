using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalObject : MonoBehaviour
{
    public float myDamage;

    [SerializeField]
    Player myPlayer;

    private void Update()
    {
        Vector3 rectangleOneScale = transform.localScale,
                rectangleTwoScale = myPlayer.transform.localScale,
                rectangleOnePosition = transform.position,
                rectangleTwoPosition = myPlayer.transform.position;

        // Calculate the sides of the rectangles in order to detect collision
        float rectangleOneRightSide = rectangleOnePosition.x - rectangleOneScale.x * .5f,
              rectangleOneLeftSide = rectangleOnePosition.x + rectangleOneScale.x * .5f,
              rectangleOneBottomSide = rectangleOnePosition.y + rectangleOneScale.y * .5f,
              rectangleOneTopSide = rectangleOnePosition.y - rectangleOneScale.y * .5f,
              rectangleTwoRightSide = rectangleTwoPosition.x - rectangleTwoScale.x * .5f,
              rectangleTwoLeftSide = rectangleTwoPosition.x + rectangleTwoScale.x * .5f,
              rectangleTwoTopSide = rectangleTwoPosition.y + rectangleTwoScale.y * .5f,
              rectangleTwoBottomSide = rectangleTwoPosition.y - rectangleTwoScale.y * .5f;

        // Collision detection between two rectangles
        if (rectangleOneRightSide < rectangleTwoLeftSide &&
            rectangleOneLeftSide > rectangleTwoRightSide &&
            rectangleOneTopSide > rectangleTwoBottomSide &&
            rectangleOneBottomSide < rectangleTwoTopSide)
        {
            myPlayer.TakeDamage(myDamage * Time.deltaTime);
        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale); 
    }
}