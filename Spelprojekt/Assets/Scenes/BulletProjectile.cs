using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private float bulletSpeed = 100;
    Vector3 previousPos;

    // Start is called before the first frame update
    void Start()
    {
        previousPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0f, 0f, bulletSpeed * Time.deltaTime);

        RaycastHit[] hits = Physics.RaycastAll(new Ray(previousPos, (transform.position - previousPos).normalized), (transform.position - previousPos).magnitude);

        previousPos = transform.position;

        for ( int bulletIndex = 0; bulletIndex < hits.Length; bulletIndex++)
        {
            Debug.Log(hits[bulletIndex].collider.gameObject.name);
        }
    }

}
