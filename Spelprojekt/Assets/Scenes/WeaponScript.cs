using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Rigidbody bulletPrefab;
    public Transform Weapon;

    void Start()
    {
        InvokeRepeating("Shoot", 0f, 1f);
    }

    void Shoot()
    {
        GameObject bullet = BulletManager.SharedInstance.GetPooledObject();
        if(bullet != null)
        {
            bullet.transform.position = Weapon.transform.position;
            bullet.transform.rotation = Weapon.transform.rotation;
            bullet.SetActive(true);
        }


    }

    void Update()
    {
        
    }
}
