using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    [SerializeField]
    Transform myRespawnPosition;
    [SerializeField]
    BoxCollider2D myCollider2D;
    [SerializeField]
    LayerMask myLayerMask;
    [SerializeField]
    ScoreManager scoreManager;
    void Update()
    {
        CheckBox();
    }
    void CheckBox()
    {
        RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, transform.localScale + Vector3.one, 0,Vector3.up, 0 , myLayerMask);
        if (hitInfo)
        {
            Debug.Log("Respawn");
            scoreManager.ResetTimer();
            hitInfo.collider.transform.position = myRespawnPosition.position;          
        }
    }


}
