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
    bool myIsLevelCompleteCollider = false;
    [SerializeField]
    ScoreManager myScoreManager;
    [SerializeField]
    GameManager myGameManager;
    private void OnValidate()
    {
        myScoreManager = FindObjectOfType<ScoreManager>();
        myGameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        CheckBox();
    }
    void CheckBox()
    {
        RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, transform.localScale + Vector3.one, 0,Vector3.up, 0 , myLayerMask);
        if (hitInfo)
        {
            if (myIsLevelCompleteCollider)
            {
                myGameManager.LevelComplete();
            }
            else
            {
                myGameManager.GameOver();

            }
        }
    }

}
