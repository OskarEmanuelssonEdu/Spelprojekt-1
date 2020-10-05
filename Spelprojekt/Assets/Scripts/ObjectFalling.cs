using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFalling : MonoBehaviour
{
    private bool isGrounded;
    Vector3 velocity = new Vector3(0, 0, 0);

    [Tooltip("Adjusts the falling speed/gravity of the falling object")]
    [SerializeField]
    public float gravity;

    [Tooltip("Distance from falling object to player to trigger fall")]
    [SerializeField]
    float myDistanceToActivate;

    [SerializeField]
    [Tooltip ("Assigned in script under OnValidate")]
    GameManager myGameManager;

    private void OnValidate()
    {
        myGameManager = FindObjectOfType<GameManager>();
    }

    void FixedUpdate()
    {
        if (CheckPlayerDistance())
        {
            CheckGrounded();
            if (!isGrounded)
            {
                velocity.y -= gravity * Time.fixedDeltaTime;
                Debug.Log("Current velocity Y = " + velocity.y);
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
        RaycastHit2D boxResult = Physics2D.BoxCast(transform.position, new Vector3(1, 1f), 0f,Vector3.down, velocity.y * Time.fixedDeltaTime);
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
