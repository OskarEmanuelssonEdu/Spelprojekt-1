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
    [Range(1, 25)]
    [Tooltip("This variable decide the speed of the bullet")]
    [SerializeField]
    float mySpeed = 10;
    [Header("Weapon Settings")]
    [Range(1, 10)]
    [Tooltip("This variable will decide how many seconds will take before the next bullet will shoot")]
    [SerializeField]
    float myTimeInBetweenShots = 2;
    float myTimerInBetweenshots = 0;
    [Range(10, 60)]
    [Tooltip("This variable will decide when the weapon will start to fire")]
    [SerializeField]
    float myDistanceToActivate = 20;

    [Header("References")]
    [SerializeField]
    BulletManager myBulletManager;
    [SerializeField]
    GameManager myGameManager;


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
            myBulletManager.GetBullet(transform.position, transform.rotation, mySpeed, damage);
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
