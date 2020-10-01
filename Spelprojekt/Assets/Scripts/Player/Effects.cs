using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(PlayerMovement))]
public class Effects : MonoBehaviour
{
    TrailRenderer myTrailRenderer;

    [Header("Trail Renderer")]

    [SerializeField]
    [Min(0)]
    float mySpeedThreshold; // default 50
    [SerializeField]
    [Tooltip("Using linear interpolation")]
    [Min(0)]
    float myTrailFadeIn; // default 1
    [SerializeField]
    [Tooltip("Using linear interpolation")]
    [Min(0)]
    float myTrailFadeAway; // default 1

    [SerializeField]
    [Min(1f)]
    float myDivider; // default 10
    [SerializeField]
    [Min(0f)]
    float myTrailRendererLeastTime; // default 0
    [SerializeField]
    [Min(0f)]
    float myTrailRendererMaxTime; // default 0.25

    public float myTrailRendererTimeValue
    {
        get
        {
            return myTrailRenderer.material.GetFloat("_TimeValue");
        }
        set
        {
            myTrailRenderer.material.SetFloat("_TimeValue", value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myTrailRenderer = GetComponent<TrailRenderer>();
        myTrailRendererTimeValue = 1f;
    }



    // Update is called once per frame
    void Update()
    {
        PlayerMovement playerMovementScript = GetComponent<PlayerMovement>();

        //float targetTime;

        //targetTime = 

        float targetTime = Mathf.Clamp(playerMovementScript.CurrentSpeed.magnitude / myDivider, myTrailRendererLeastTime, myTrailRendererMaxTime);

        if (playerMovementScript.CurrentSpeed.magnitude > mySpeedThreshold)
        {
            myTrailRenderer.time = Mathf.MoveTowards(myTrailRenderer.time, targetTime, myTrailFadeIn * Time.fixedDeltaTime);
            //myTrailRendererTimeValue = Mathf.MoveTowards(myTrailRendererTimeValue, targetTime, myTrailFadeAway * Time.deltaTime);
        }
        else
        {
            myTrailRenderer.time = Mathf.MoveTowards(myTrailRenderer.time, myTrailRendererLeastTime, myTrailFadeAway * Time.fixedDeltaTime);

            //myTrailRendererTimeValue = Mathf.MoveTowards(myTrailRendererTimeValue, myTrailRendererLeastTime, myTrailFadeAway * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            //myTrailRenderer.material.SetFloat("_TimeValue", 1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //myTrailRenderer.material.SetFloat("_TimeValue", 0);
        }
    }
}
