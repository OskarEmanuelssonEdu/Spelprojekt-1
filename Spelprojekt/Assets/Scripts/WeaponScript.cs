using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField]
    float damage = 10f;
    [SerializeField]
    float range = 100f;
    [SerializeField]
    float mySpeed = 10;
    [SerializeField]
    float myTimeInBetweenShots = 2;
    float myTimerInBetweenshots = 0;

    [SerializeField]
    BulletManager myBulletManager;
    [SerializeField]
    GameManager myGameManager;

    void Start()
    {


    }

    void Update()
    {
        if (myTimerInBetweenshots >= myTimeInBetweenShots)
        {
            myBulletManager.GetBullet(transform.position, Quaternion.identity, mySpeed, damage);
            myTimerInBetweenshots = 0;
        }
        else
        {
            myTimerInBetweenshots = myTimerInBetweenshots + Time.deltaTime;
        }
    }
}
