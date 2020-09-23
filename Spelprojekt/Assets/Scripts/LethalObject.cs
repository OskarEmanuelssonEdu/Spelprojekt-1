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
        Vector3 RectangleOneScale = transform.localScale,
                RectangleTwoScale = myPlayer.transform.localScale,
                RectangleOnePosition = transform.position,
                RectangleTwoPosition = myPlayer.transform.position;

        float RectangleOneRightSide = RectangleOnePosition.x - RectangleOneScale.x * .5f,
              RectangleOneLeftSide = RectangleOnePosition.x + RectangleOneScale.x * .5f,
              RectangleOneBottomSide = RectangleOnePosition.y + RectangleOneScale.y * .5f,
              RectangleOneTopSide = RectangleOnePosition.y - RectangleOneScale.y * .5f,
              RectangleTwoRightSide = RectangleTwoPosition.x - RectangleTwoScale.x * .5f,
              RectangleTwoLeftSide = RectangleTwoPosition.x + RectangleTwoScale.x * .5f,
              RectangleTwoTopSide = RectangleTwoPosition.y + RectangleTwoScale.y * .5f,
              RectangleTwoBottomSide = RectangleTwoPosition.y - RectangleTwoScale.y * .5f;

        // Collision detection between two rectangles
        if (RectangleOneRightSide < RectangleTwoLeftSide &&
            RectangleOneLeftSide > RectangleTwoRightSide &&
            RectangleOneTopSide > RectangleTwoBottomSide &&
            RectangleOneBottomSide < RectangleTwoTopSide)
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