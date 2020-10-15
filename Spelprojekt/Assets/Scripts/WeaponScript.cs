using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [Header("Bullet Settings")]
    [Range(0, 25)]
    [Tooltip("This variable will decide the damage the bullet will do to the player")]
    [SerializeField]
    float damage = 10f;
    [Range(1, 50)]
    [Tooltip("This variable decide the speed of the bullet")]
    [SerializeField]
    float mySpeed = 10;
    [Range(1,10)]
    [SerializeField]
    float myScale = 1;
    [Header("Weapon Settings")]
    [Range(1, 10)]
    [Tooltip("This variable will decide how many seconds will take before the next bullet will shoot")]
    [SerializeField]
    float myTimeInBetweenShots = 2;
    float myTimerInBetweenshots = 0;
    [Tooltip("This variable will decide when the weapon will start to fire")]
    [SerializeField]
    float myDistanceToActivate;
    [SerializeField]
    float bulletLifeTime = 10;
    [Header("References")]
    [SerializeField]
    BulletManager myBulletManager;
    [SerializeField]
    GameManager myGameManager;
    [Header("SOUND")]
    [SerializeField]
    private AudioClip myShootClip;
    [SerializeField]
    private float myShootVolume = 1;


    void OnValidate()
    {
        myBulletManager = FindObjectOfType<BulletManager>();
        myGameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        if (CheckPlayerDistance())
        {
          Shoot();
        }
    }


    void Shoot()
    {
        if (myTimerInBetweenshots >= myTimeInBetweenShots)
        {
            AudioManager.ourPublicInstance.PlaySFX1(myShootClip,myShootVolume);
            myBulletManager.GetBullet(transform.position, transform.rotation, mySpeed, damage, bulletLifeTime, myScale);
            myTimerInBetweenshots = 0;
        }
        else
        {
            myTimerInBetweenshots = myTimerInBetweenshots + Time.deltaTime;
        }
    }

    bool CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, myGameManager.PlayerPosition());
        if (distanceToPlayer < myDistanceToActivate)
        {
            return true;
        }
        return false;
    }
}
