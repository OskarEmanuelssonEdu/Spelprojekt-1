using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BulletProjectile : MonoBehaviour
{
    [SerializeField]
    private AudioClip myBulletDestroySound;
    [SerializeField]
    private float myBulletDestroyVolume;
    public float myBulletSpeed;
    public float myBulletDamage;
 
 
    [SerializeField]
    LayerMask myLayerMask;
    public BulletManager myBulletManager;
    public GameManager myGameManager;
    public Player myPlayer;

    float myLifeTimer = 0;
    public float myLifeTime = 10;

    private void Update()
    {
        if (myLifeTimer>= myLifeTime)
        {
            AudioManager.ourPublicInstance.PlaySFX1(myBulletDestroySound, myBulletDestroyVolume);
            myBulletManager.ReturnBullet(this);
            myLifeTimer = 0;
        }
        else
        {
            myLifeTimer += Time.deltaTime;
        }
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



}
