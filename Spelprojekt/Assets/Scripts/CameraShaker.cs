using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float myShakeRadius = 1.0f;
    float duration = 1.0f;
    public float mySlowDownAmonut = 1.0f;
    [SerializeField]
    float myInitialDuration;
    [SerializeField]
    NewCameraMovement myNewCameraMovement;

    public bool myShouldShake = false;
    public Transform myCamera;

    Vector3 myStartPosition;


    // Start is called before the first frame update
    private void OnValidate()
    {

        myNewCameraMovement = FindObjectOfType<NewCameraMovement>();

    }
    void Start()
    {
        myCamera = Camera.main.transform;
        myStartPosition = myCamera.localPosition;
        myInitialDuration = duration;
    }

    // Update is called once per frame
    void Update()
    {
        myCamera = Camera.main.transform;
        myStartPosition = myCamera.localPosition;

        if (myShouldShake)
        {
            if (duration > 0)
            {
                myCamera.localPosition = myStartPosition + Random.insideUnitSphere * myShakeRadius;
                duration -= Time.deltaTime * mySlowDownAmonut;
            }
            else
            {
                myShouldShake = false;
                duration = myInitialDuration;
                myCamera.localPosition = new Vector3(myStartPosition.x, myStartPosition.y, -15);
            }
        }
    }
}