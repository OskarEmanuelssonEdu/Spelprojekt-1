using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFalling : MonoBehaviour
{
    private bool isGrounded;
    private bool isFalling;

    [Tooltip("Adjusts the falling speed of the lethal object")]
    [SerializeField]
    float speed;

    [Tooltip("Adjusts the distance which the player has to be from the falling object to trigger fall")]
    [SerializeField]
    float myDistanceToActivate = 10;

    GameManager myGameManager;

    private void OnValidate()
    {
        myGameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {

        if (CheckPlayerDistance())
        {
            CheckGrounded();
            if (!isGrounded)
            {
                transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
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
        RaycastHit2D boxResult = Physics2D.BoxCast(transform.position, new Vector3(1, 0.5f), 0f, new Vector2(0, -1), 0.25f);
        if (boxResult.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
