using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float shakeRadius = 1.0f;
    float duration = 1.0f;
    public float slowDownAmonut = 1.0f;
    [SerializeField]
    float initialDuration;
    [SerializeField]
    NewCameraMovement myNewCameraMovement;

    public bool shouldShake = false;
    public Transform camera;

    Vector3 startPosition;


    // Start is called before the first frame update
    private void OnValidate()
    {

        myNewCameraMovement = FindObjectOfType<NewCameraMovement>();

    }
    void Start()
    {
        camera = Camera.main.transform;
        startPosition = camera.localPosition;
        initialDuration = duration;
    }

    // Update is called once per frame
    void Update()
    {
        camera = Camera.main.transform;
        startPosition = camera.localPosition;

        if (shouldShake)
        {
            if (duration > 0)
            {
                camera.localPosition = startPosition + Random.insideUnitSphere * shakeRadius;
                duration -= Time.deltaTime * slowDownAmonut;
            }
            else
            {
                shouldShake = false;
                duration = initialDuration;
                camera.localPosition = new Vector3(startPosition.x, startPosition.y, -15);
            }
        }
    }
}