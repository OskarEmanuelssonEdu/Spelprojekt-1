using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappelHook : MonoBehaviour
{
    [Header("Grappling hook settings")]
    [Range(10, 30)]
    [SerializeField]
    float myRange = 20;
    [Range(1,3)]
    [SerializeField]
    float myPullForce = 1.4f;
    [Range(0, 5)]
    [SerializeField]
    float myRangeToBreak = 2.5f;
   
    bool myDoGrappel = false;
    Vector3 myHitPosition;
    Vector3 myMousePosition;
    [SerializeField]
    LayerMask myLayerMask;
    
    [Header("Refernce settings")]
    [SerializeField]
    Transform myShootPosition;
    [SerializeField]
    PlayerMovement myPlayerMovement;
    [SerializeField]
    LineRenderer myLineRenderer;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootRay();
            
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            myLineRenderer.enabled = false;
            myDoGrappel = false;
        }      
    }
    void FixedUpdate()
    {
        if (myDoGrappel)
        {
            DoGrappelPhysics();
        }
    }
    void ShootRay()
    {
        myMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDir = transform.position - new Vector3(myMousePosition.x, myMousePosition.y , 0);
        RaycastHit2D hitInfo = Physics2D.Raycast(myShootPosition.position, -mouseDir.normalized, myRange, myLayerMask);

        if (hitInfo.collider != null)
        {
            myLineRenderer.enabled = true;
            myHitPosition = hitInfo.centroid;
            myDoGrappel = true;
        }
    }
    void DoGrappelPhysics()
    {

        Vector3 directionToPivot = transform.position - myHitPosition;
        Debug.DrawLine(transform.position, myHitPosition);

        myPlayerMovement.ApplyForce(-directionToPivot.normalized * myPullForce);

  
        //Line renderer points
        myLineRenderer.SetPosition(0, transform.position);
        myLineRenderer.SetPosition(1, myHitPosition);

        if (directionToPivot.magnitude >= myRange + 5 || directionToPivot.magnitude <= myRangeToBreak)
        {
            myLineRenderer.enabled = false;
            myDoGrappel = false;
        }
    }
}
