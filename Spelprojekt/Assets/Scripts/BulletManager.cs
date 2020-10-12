using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    List<BulletProjectile> myActiveBullets;
    List<BulletProjectile> myPassiveBullets;
    [Header("Bullet Settings")]
    [Range(150,400)]
    [SerializeField]
    int myBulletAmount = 100;

    [Header("References")]
    [SerializeField]
    GameManager myGameManager;
    [SerializeField]
    BulletProjectile myBulletPrefab;
    [SerializeField]
    Player myPlayer;

    private void OnValidate()
    {
        myGameManager = FindObjectOfType<GameManager>();
        myPlayer = FindObjectOfType<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {
        myActiveBullets = new List<BulletProjectile>(myBulletAmount);
        myPassiveBullets = new List<BulletProjectile>(myBulletAmount);
        InitializeBullets(myGameManager);
    }

    //Vrf Ref till GameManageR?
    void InitializeBullets(GameManager aGameManager)
    {
        for (int bulletIndex = 0; bulletIndex < myBulletAmount; bulletIndex++)
        {
            BulletProjectile bullet = Instantiate(myBulletPrefab, Vector3.zero, Quaternion.identity, transform);
            //Sätter refernser till bulleten
            bullet.myBulletManager = this;
            bullet.myPlayer = myPlayer;
            bullet.myGameManager = aGameManager;
           
            myPassiveBullets.Add(bullet);
            bullet.gameObject.SetActive(false);
        }
    }
    void AddBulletsToPassiveList()
    {
        for (int bulletIndex = 0; bulletIndex < myBulletAmount; bulletIndex++)
        {
            myPassiveBullets.Add(myActiveBullets[bulletIndex]);
            myActiveBullets.Remove(myActiveBullets[bulletIndex]);
        }
        DisableBullets();
    }
    void DisableBullets()
    {
        for (int bulletIndex = 0; bulletIndex < myBulletAmount; bulletIndex++)
        {
            myPassiveBullets[bulletIndex].gameObject.SetActive(false);
        }
    }
    public void GetBullet(Vector2 aPosition, Quaternion aRotation, float aSpeed, float aDamage)
    {
        myPassiveBullets[myPassiveBullets.Count - 1].gameObject.SetActive(true);
        myPassiveBullets[myPassiveBullets.Count - 1].transform.position = aPosition;
        myPassiveBullets[myPassiveBullets.Count - 1].transform.rotation = aRotation;
        myPassiveBullets[myPassiveBullets.Count - 1].myBulletSpeed = aSpeed;
        myPassiveBullets[myPassiveBullets.Count - 1].myBulletDamage = aDamage;

        myActiveBullets.Add(myPassiveBullets[myPassiveBullets.Count - 1]);
        myPassiveBullets.RemoveAt(myPassiveBullets.Count - 1);
    }
    public void ReturnBullet(BulletProjectile aBullet)
    {
        aBullet.transform.position = Vector3.zero;
        myActiveBullets.Remove(aBullet);
        myPassiveBullets.Add(aBullet);
        aBullet.gameObject.SetActive(false);
    }
}
