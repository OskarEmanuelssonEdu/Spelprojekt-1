using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappelHook : MonoBehaviour
{
    [SerializeField]
    private AudioClip myGrappleSound;
    [SerializeField]
    [Range(0, 1.0f)]
    private float myGrappleSoundVolume;
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

    [Range(0,1)]
     [SerializeField]
    float myUpAcceleration = 1;
    float offsetToStartAddUpAcceleration = 4f;
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
    [SerializeField]
    Camera myCamera;
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
        myMousePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDir = transform.position - new Vector3(myMousePosition.x, myMousePosition.y , 0);
        RaycastHit2D hitInfo = Physics2D.Raycast(myShootPosition.position, -mouseDir.normalized, myRange, myLayerMask);

        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer != 0)
        {
            myLineRenderer.enabled = true;
            myHitPosition = hitInfo.point;
            myDoGrappel = true;
            AudioManager.ourPublicInstance.PlaySFX1(myGrappleSound, myGrappleSoundVolume);
        }
    }
    void DoGrappelPhysics()
    {

        Vector3 directionToPivot = transform.position - myHitPosition;
        Debug.DrawLine(transform.position, myHitPosition);


        if (directionToPivot.y >= -offsetToStartAddUpAcceleration && directionToPivot.y <=  offsetToStartAddUpAcceleration && myPlayerMovement.CurrentSpeed.y > 0)
        {
            myPlayerMovement.ApplyForce(-directionToPivot.normalized * myPullForce + (Vector3.up * myUpAcceleration) * directionToPivot.magnitude * myUpAcceleration );

        }
        else
        {
            myPlayerMovement.ApplyForce(-directionToPivot.normalized * myPullForce * Mathf.Clamp( directionToPivot.magnitude, 1 , directionToPivot.magnitude * 0.1f));
        }
        //Line renderer points
        myLineRenderer.SetPosition(0, transform.position);
        myLineRenderer.SetPosition(1, myHitPosition);

        if (directionToPivot.magnitude >= myRange + 7 || directionToPivot.magnitude <= myRangeToBreak )
        {
            myLineRenderer.enabled = false;
            myDoGrappel = false;
        }
    }
}
