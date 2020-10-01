using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookBoohyah : MonoBehaviour
{

    [SerializeField]
    GrapplingProjectile myProjectilePrefab;
    GrapplingProjectile myProjectile;

    [SerializeField]
    float myProjectileLifeTime = 3;
    float myProjectileLifeTimer = 0;
    Vector3 myMousePosition;
    Vector3 myMouseDirection;
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
    [Range(10, 100)]
    [SerializeField]
    float myProjectileSpeed;
   
    [SerializeField]
    Camera myOrtograpicCamera;
    [SerializeField]
    Transform myShootPosition;
    [SerializeField]
    LineRenderer myLineRenderer;

    [SerializeField]
    KeyCode myGrappleKey = KeyCode.Mouse0;

    bool myHit = true;
    public bool Hit
    {
        set
        {
            myHit = value;
        }
    }

    void Start()
    {
        if (myProjectile == null)
        {
            myProjectile = Instantiate(myProjectilePrefab, Vector3.zero, Quaternion.identity, transform);
            myProjectile.GrapplingHook = this;

        }
        myProjectile.gameObject.SetActive(false);
    }
    void Update()
    {
        GetInputs();
        
       
        if (!myHit && myProjectile.gameObject.activeSelf)
        {
            if (myProjectileLifeTimer >= myProjectileLifeTime)
            {
                myProjectile.gameObject.SetActive(false);
                myProjectileLifeTimer = 0;
                Debug.Log("Lifetime out");
            }
            else
            {
                myProjectileLifeTimer += Time.deltaTime * myProjectileSpeed;
                Debug.Log("Run Counter , Timer: " + myProjectileLifeTimer);
            }
        }
        Vector3 tempGrapplingPos = myProjectile.MoveProjectile(myMouseDirection, myProjectileSpeed, myGrappleLayer);
        
        if (tempGrapplingPos != Vector3.zero&& !myGrappling) 
        {           
            myGrapplePosition = tempGrapplingPos;
            myGrappleDistance = (tempGrapplingPos - transform.position).magnitude + myGrappleStartSlack;
            myHit = false;
            myProjectile.gameObject.SetActive(false);

            myGrappling = true;
        }
        
    }
    private void FixedUpdate()
    {
        Grapple();

    }
    void OnValidate()
    {

        myPlayerMovement = FindObjectOfType<PlayerMovement>();
    }
    

    void GetInputs()
    {
        if (Input.GetKeyDown(myGrappleKey))
        {
            myProjectileLifeTimer = 0;
            myProjectile.transform.position = myShootPosition.position;
            myProjectile.gameObject.SetActive(true);
            myHit = false;
            myMousePosition = myOrtograpicCamera.ScreenToWorldPoint(Input.mousePosition);
            myMouseDirection = new Vector3(myMousePosition.x, myMousePosition.y, 0) - transform.position;
          
           
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



                myPlayerMovement.CurrentSpeed = Vector3.Lerp(myPlayerMovement.CurrentSpeed, Vector3.Project(myPlayerMovement.CurrentSpeed, Quaternion.Euler(0, 0, 90) * ((myGrapplePosition - transform.position).normalized)), mySwingCorrection);
                myPlayerMovement.CurrentSpeed += ((transform.position - myGrapplePosition).normalized * myGrappleDistance) * (myRopeStrength * (myGrappleDistance - (myPlayerMovement.transform.position - myGrapplePosition).magnitude)) * Time.fixedDeltaTime;
                myPlayerMovement.CurrentSpeed += myPlayerMovement.CurrentSpeed.normalized * myGrappleSpeedIncrease * Time.fixedDeltaTime;

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
