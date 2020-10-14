using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BulletProjectile : MonoBehaviour
{
    public float myBulletSpeed;
    public float myBulletDamage;
    private float LifeTime = 5f;
 
    [SerializeField]
    //LayerMask myLayerMask;
    public BulletManager myBulletManager;
    public GameManager myGameManager;
    public Player myPlayer;
    LayerMask mask = LayerMask.GetMask("Player");

    private void Start()
    {
        Destroy(this.gameObject, LifeTime);
    }

    void Update()
    {
        CheckIfHit();
        
    }
    private void FixedUpdate()
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
        RaycastHit2D hits = Physics2D.BoxCast(transform.position, transform.localScale, 0,transform.forward, myBulletSpeed * Time.deltaTime , mask);


        if (hits.collider != null && hits.collider.gameObject.layer != 0)
        {
            myPlayer.TakeDamage(myBulletDamage);
            myBulletManager.ReturnBullet(this);
            Destroy(this.gameObject);
        }

    }


}
