using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalObject : MonoBehaviour
{
    public string myTargetTag;
    public float myDamage;

    private void OnCollisionEnter(Collision aGameObject)
    {
        if (aGameObject.gameObject.CompareTag(myTargetTag))
        {
            aGameObject.gameObject.GetComponent<IDamageable>().TakeDamage(myDamage);
        }
    }
}