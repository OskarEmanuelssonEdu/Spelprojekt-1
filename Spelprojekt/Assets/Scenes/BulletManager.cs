using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager SharedInstance;
    public List<GameObject> pooledBullets;
    public GameObject myBullets;
    public int amountBullets;

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //List containing bullets
        pooledBullets = new List<GameObject>();
        //Instantiate myBullets according to number in amountBullets
        //Bullets set to inactive state before adding them to list
        for(int i = 0; i < amountBullets; i++)
        {
            GameObject bullet = (GameObject)Instantiate(myBullets);
            bullet.SetActive(false);
            pooledBullets.Add(bullet);
        }
    }

    //Called from other scripts to utilize the Bullet Pool
    //Allows other scripts to set objects as active/inactive during runtime
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledBullets.Count; i++)
        {
            if (!pooledBullets[i].activeInHierarchy)
            {
                return pooledBullets[i];
            }
        }
        return null;
    }
}
