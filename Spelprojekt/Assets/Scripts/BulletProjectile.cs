using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private float bulletSpeed = 20;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0f, 0f, bulletSpeed * Time.deltaTime);

        RaycastHit2D hits = Physics2D.Raycast(transform.position, Vector2.right);

    }

}
