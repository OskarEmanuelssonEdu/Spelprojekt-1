using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    float myGravity = -9.82f;
    float myDistToGround;
    [SerializeField]
    LayerMask myLayerMask;
    [SerializeField]
    float myDistanceToActivate = 10;
    [SerializeField]
    GameManager myGameManager;

    // Start is called before the first frame update
    void FixedUpdate()
    {
        //CheckIfHit();
        CheckForGround();
        if(CheckPlayerDistance())
        {
            Fall();
        }
    }
    private void Fall()
    {
        transform.Translate(0f, myGravity * Time.fixedDeltaTime, 0f);
    }

    //private void CheckIfHit()
    //{
    //    RaycastHit2D hits = Physics2D.BoxCast(transform.position, transform.localScale, 0, new Vector2(0,-9));
    //}
    bool CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, myGameManager.PlayerPosition());
        if (distanceToPlayer < myDistanceToActivate)
        {
            return true;
        }
        return false;
    }
    void CheckForGround()
    {
        if (transform.position.y < 0)
        {
            Vector3 position = transform.position;
            position.y = 0f;
            transform.position = position;
        }
        
    }
}

