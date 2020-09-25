using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookBoohyah : MonoBehaviour
{

    Vector3 myMousePosition;
    Vector3 myGrapplePosition;

    [SerializeField]
    float myGrappleMaxDistance;

    float myGrappleDistance;

    [SerializeField]
    LayerMask myGrappleLayer;
    [SerializeField]
    PlayerMovement myPlayerMovement;

    [SerializeField]
    float myGrappleStartSlack;
    bool myGrappling;

    [SerializeField]
    float myRopeStrength;

    [SerializeField]
    float myGrappleSpeedIncrease;

    [SerializeField]
    float mySwingCorrection;

    [SerializeField]
    Camera myOrtograpicCamera;


    [SerializeField]
    LineRenderer myLineRenderer;

    [SerializeField]
    KeyCode myGrappleKey = KeyCode.Mouse0;

    void Start()
    {

    }
    private void Update()
    {

        GetInputs();

    }
    private void FixedUpdate()
    {

        Grapple();

    }

    void GetInputs()
    {

        myMousePosition = myOrtograpicCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, myMousePosition - transform.position, myGrappleMaxDistance, myGrappleLayer);


        if (Input.GetKeyDown(myGrappleKey) && hit.collider != null)
        {


            myGrapplePosition = new Vector3(hit.point.x, hit.point.y, 0);
            myGrappleDistance = (myGrapplePosition - transform.position).magnitude + myGrappleStartSlack;
            myGrappling = true;



        }
        else if (Input.GetKeyUp(myGrappleKey))
        {

            myGrappling = false;

        }

    }

    void Grapple()
    {


        if (myGrappling)
        {
            myLineRenderer.enabled = true;
            myLineRenderer.SetPosition(0, transform.position);
            myLineRenderer.SetPosition(1, myGrapplePosition);


            if ((myPlayerMovement.transform.position - myGrapplePosition).magnitude >= myGrappleDistance)
            {
     


                myPlayerMovement.MyCurrentVelocity = Vector3.Lerp(myPlayerMovement.MyCurrentVelocity, Vector3.Project(myPlayerMovement.MyCurrentVelocity, Quaternion.Euler(0, 0, 90) * ((myGrapplePosition - transform.position).normalized)), mySwingCorrection);
                myPlayerMovement.MyCurrentVelocity += ((transform.position - myGrapplePosition).normalized * myGrappleDistance) * (myRopeStrength * (myGrappleDistance - (myPlayerMovement.transform.position - myGrapplePosition).magnitude)) * Time.fixedDeltaTime;
                myPlayerMovement.MyCurrentVelocity += myPlayerMovement.MyCurrentVelocity.normalized * myGrappleSpeedIncrease * Time.fixedDeltaTime;

                print((myGrappleDistance - (myPlayerMovement.transform.position - myGrapplePosition).magnitude));
            }




            Debug.DrawRay(myGrapplePosition, ((myGrapplePosition - transform.position).normalized) * myGrappleDistance, Color.red);


        }
        else
        {

            myLineRenderer.enabled = false;

        }



    }

}
