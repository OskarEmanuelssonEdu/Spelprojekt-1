using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingProjectile : MonoBehaviour
{

    
    GrappleHookBoohyah grapplingHook;
    [SerializeField]
    TrailRenderer trailRenderer;
    public GrappleHookBoohyah GrapplingHook
    {
        set
        {
            grapplingHook = value;
        }
    }
    private void OnEnable()
    {
        trailRenderer.Clear();
    }
    public Vector3 MoveProjectile(Vector3 aDirection, float aProjectileSpeed, LayerMask aLayer, float aProjectileMaxDistance)
    {

        if (gameObject.activeSelf)
        {
            Debug.DrawRay(transform.position, aDirection.normalized * aProjectileSpeed * Time.deltaTime, Color.green, Mathf.Infinity);
            transform.Translate(aDirection.normalized * aProjectileSpeed * Time.deltaTime);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, aDirection.normalized, (aProjectileSpeed * Time.deltaTime), aLayer);

            if (hit.collider != null && hit.collider.gameObject.layer != 0)
            {
                gameObject.SetActive(false);
         
                return new Vector3(hit.point.x, hit.point.y, 0);
            
            }
            else
            {
                float dist = Vector3.Distance(transform.position, grapplingHook.transform.position);
                if (dist >= aProjectileMaxDistance)
                {
                    gameObject.SetActive(false);
              
                    return Vector3.zero;
                }

                if (hit.collider != null && hit.collider.gameObject.layer == 0)
                {
                   gameObject.SetActive(false);
                }
          
                return Vector3.zero;
            }
        }
        return Vector3.zero;

    }
}
