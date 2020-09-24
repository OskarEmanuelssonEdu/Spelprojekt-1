using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public float myBulletSpeed;
    public float myBulletDamage;
    public float myDamage;
    public BulletManager myBulletManager;
    public GameManager myGameManager;




    void Update()
    {
        
        Move();
 
    }

    //Moves Bullet forward
    private void Move()
    {
        transform.Translate(0f, 0f, myBulletSpeed * Time.deltaTime);
    }

    //Checks if bullet hits
    private void CheckIfHit()
    {
        RaycastHit2D hits = Physics2D.Raycast(transform.position, -transform.right, myBulletSpeed*Time.deltaTime);

        if (hits.collider.GetComponent<Player>() != null)
        {
            hits.collider.GetComponent<Player>().TakeDamage(myDamage);
            myBulletManager.ReturnBullet(this);
        }

    }


}
