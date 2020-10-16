using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class GrappleHookBoohyah : MonoBehaviour
{
    [SerializeField]
    private AudioClip myGrappleShootSound;
    [SerializeField]
    [Range(0, 1.0f)]
    private float myGrappleShootSoundVolume = 1f;
    [SerializeField]
    private AudioClip myGrappleHitSound;
    [SerializeField]
    [Range(0, 1.0f)]
    private float myGrappleHitSoundVolume = 1f;


    [Header("Projectile Settings")]
    [SerializeField]   
    GrapplingProjectile myProjectilePrefab;
    GrapplingProjectile myProjectile;
    [SerializeField]
    [Tooltip("Berättar vart skottet kommer att skjutas från")]
    Transform myShootPosition;

    [Tooltip("Definerar hur snabbt projektilen kommer att färdas")]
    [Range(10, 100)]
    [SerializeField]
    float myProjectileSpeed;
    
    [Range(10, 100)]
    [Tooltip("Max distansen som pojektilen kommer att färdas (I UNITS)")]
    [SerializeField]
    float myGrappleMaxDistance;

    Vector3 myMousePosition;
    Vector3 myMouseDirection;
    Vector3 myGrapplePosition;

    [SerializeField]


    float myGrappleDistance;

    [Header("Rope settings")]
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


    [Header("Vfx settings")]
    [SerializeField]
    VisualEffect myHitEffect;
    [SerializeField]
    Camera myOrtograpicCamera;
    [SerializeField]
    LineRenderer myLineRenderer;
    [SerializeField]
    Animator animator;

    [SerializeField]
    KeyCode myGrappleKey = KeyCode.Mouse0;

    public Vector3 ShootPosition
    {
        get
        {
            return myShootPosition.position;
        }
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (myProjectile == null)
        {
            myProjectile = Instantiate(myProjectilePrefab, Vector3.zero, Quaternion.identity);
            myProjectile.GrapplingHook = this;
            myProjectile.Line = myLineRenderer;
        }
        myProjectile.gameObject.SetActive(false);
    }
    void Update()
    {
        GetInputs();
        
        Vector3 tempGrapplingPos = myProjectile.MoveProjectile(myMouseDirection, myProjectileSpeed, myGrappleLayer, myGrappleMaxDistance);
        
        if (tempGrapplingPos != Vector3.zero&& !myGrappling) 
        {           
            myGrapplePosition = tempGrapplingPos;
            myGrappleDistance = (tempGrapplingPos - transform.position).magnitude + myGrappleStartSlack;
            myHitEffect.transform.position = myGrapplePosition;
            myHitEffect.Play();
            myProjectile.gameObject.SetActive(false);

            myGrappling = true;
            AudioManager.ourPublicInstance.PlaySFX1(myGrappleHitSound, myGrappleHitSoundVolume);
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
        if (Input.GetKeyDown(myGrappleKey) && !myProjectile.gameObject.activeSelf && !myGrappling)
        {
            AudioManager.ourPublicInstance.PlaySFX1(myGrappleShootSound, myGrappleShootSoundVolume);
            myProjectile.transform.position = myShootPosition.position;
            myProjectile.gameObject.SetActive(true);
            
            myMousePosition = myOrtograpicCamera.ScreenToWorldPoint(Input.mousePosition);
            myMouseDirection = new Vector3(myMousePosition.x, myMousePosition.y, 0) - transform.position;
          
           
        }
        else if (Input.GetKeyUp(myGrappleKey))
        {
            myLineRenderer.gameObject.SetActive(false);
            myGrappling = false;
            
        }
    }
  
    void Grapple()
    {


        if (myGrappling)
        {
            
            
            animator.SetBool("isGrappling", true);
            myLineRenderer.enabled = true;
            myLineRenderer.SetPosition(0, myShootPosition.position + myPlayerMovement.CurrentSpeed * Time.fixedDeltaTime);
            myLineRenderer.SetPosition(1, myGrapplePosition);


            if ((myPlayerMovement.transform.position - myGrapplePosition).magnitude >= myGrappleDistance)
            {


               
                myPlayerMovement.CurrentSpeed = Vector3.Lerp(myPlayerMovement.CurrentSpeed, Vector3.Project(myPlayerMovement.CurrentSpeed, Quaternion.Euler(0, 0, 90) * ((myGrapplePosition - transform.position).normalized)), mySwingCorrection);
                myPlayerMovement.CurrentSpeed += ((myGrapplePosition - transform.position).normalized/* * myGrappleDistance*/) * Mathf.Pow(myRopeStrength, Mathf.Abs((myGrappleDistance - (myGrapplePosition - myPlayerMovement.transform.position).magnitude)) * Time.fixedDeltaTime);
                myPlayerMovement.CurrentSpeed += myPlayerMovement.CurrentSpeed.normalized * myGrappleSpeedIncrease * Time.fixedDeltaTime;

                //print((myGrappleDistance - (myPlayerMovement.transform.position - myGrapplePosition).magnitude));
            }




            Debug.DrawRay(myGrapplePosition, ((myGrapplePosition - transform.position).normalized) * myGrappleDistance, Color.red);
            

        }
        else
        {
            animator.SetBool("isGrappling", false);
            
            //myLineRenderer.enabled = false;

        }



    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(myShootPosition.position, myGrappleMaxDistance);

    }
}
