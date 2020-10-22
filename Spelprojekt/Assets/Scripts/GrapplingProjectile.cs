using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingProjectile : MonoBehaviour
{

    
    GrappleHookBoohyah grapplingHook;
    LineRenderer myLineRenderer;
    public GrappleHookBoohyah GrapplingHook
    {
        set
        {
            grapplingHook = value;
        }
    }
    public LineRenderer Line
    {
        set
        {
            myLineRenderer = value;
        }
    }
    private void OnEnable()
    {
       
        //trailRenderer.Clear();
    }
    public Vector3 MoveProjectile(Vector3 aDirection, float aProjectileSpeed, LayerMask aLayer, float aProjectileMaxDistance)
    {

        if (gameObject.activeSelf)
        {
            myLineRenderer.gameObject.SetActive(true);
            myLineRenderer.SetPosition(0, grapplingHook.ShootPosition);
            myLineRenderer.SetPosition(1, transform.position + aDirection.normalized * (aProjectileSpeed * Time.deltaTime));

            Debug.DrawRay(transform.position, aDirection.normalized * aProjectileSpeed * Time.deltaTime, Color.green, Mathf.Infinity);
            transform.Translate(aDirection.normalized * aProjectileSpeed * Time.deltaTime);
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1, aDirection.normalized, (aProjectileSpeed * Time.deltaTime), aLayer);

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
                    myLineRenderer.gameObject.SetActive(false);
                    return Vector3.zero;
                }

                if (hit.collider != null && hit.collider.gameObject.layer == 0)
                {
                    myLineRenderer.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
          
                return Vector3.zero;
            }
        }
        return Vector3.zero;

    }
}
