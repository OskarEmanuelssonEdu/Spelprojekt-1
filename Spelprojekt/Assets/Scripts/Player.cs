using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float myMaxHlaeth = 10;
    public float myCurrentHealth = 0;

    [SerializeField]
    GameManager myGameManager;
    // Start is called before the first frame update
    void Start()
    {
        myCurrentHealth = myMaxHlaeth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float someDanmage)
    {
        myCurrentHealth = myCurrentHealth - someDanmage;
        if (myCurrentHealth <= 0)
        {
          
        }
    }

    public void GiveHealth(float someHealthToGive)
    {
        myCurrentHealth = myCurrentHealth + someHealthToGive;
    }
}
