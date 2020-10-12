using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BulletProjectile : MonoBehaviour
{
    public float myBulletSpeed;
    public float myBulletDamage;
 
    [SerializeField]
    LayerMask myLayerMask;
    public BulletManager myBulletManager;
    public GameManager myGameManager;
    public Player myPlayer;


    void Update()
    {
        CheckIfHit();
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
        RaycastHit2D hits = Physics2D.BoxCast(transform.position, transform.localScale, 0,transform.forward, myBulletSpeed * Time.deltaTime , myLayerMask);

        if (hits.collider != null && hits.collider.gameObject.layer != 0)
        {
            myPlayer.TakeDamage(myBulletDamage);
            myBulletManager.ReturnBullet(this);
        }

    }


}
