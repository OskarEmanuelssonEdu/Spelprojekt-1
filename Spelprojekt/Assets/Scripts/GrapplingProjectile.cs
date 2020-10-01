using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingProjectile : MonoBehaviour
{
    GrappleHookBoohyah grapplingHook;
    public  GrappleHookBoohyah GrapplingHook
    {
        set
        {
            grapplingHook = value;
        }
    }
    public Vector3 MoveProjectile(Vector3 aDirection, float aProjectileSpeed, LayerMask aLayer)
    {
        transform.Translate(aDirection.normalized * aProjectileSpeed * Time.deltaTime);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, aDirection.normalized, aProjectileSpeed * Time.deltaTime, aLayer);

        if (hit.collider != null && hit.collider.gameObject.layer != 0)
        {
            gameObject.SetActive(false);
            grapplingHook.Hit = true;
            return new Vector3(hit.point.x, hit.point.y, 0);
            
        }
        else
        {
            grapplingHook.Hit = false;
            return Vector3.zero;
        }


    }
}
