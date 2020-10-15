using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    [Tooltip("THIS WILL BE AUTOMATICALLY FILLED")]
    private Player myPlayer;

    [SerializeField]
    [Tooltip("THIS WILL BE AUTOMATICALLY FILLED")]
    private LevelManager myLevelManager;

    [SerializeField]
    [Tooltip("Range ...")]
    [Min(0)]
    private float myRange;

    private bool myPlayerHasEnteredCheckpoint = false;

    private void OnValidate()
    {
        myPlayer = FindObjectOfType<Player>();
        myLevelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        CheckPointColideCheck();

        if (
            myPlayer.transform.position.x >= transform.position.x
            && myPlayer.transform.position.x <= (transform.position.x + myRange)
            && !myPlayerHasEnteredCheckpoint
        )
        {
            myLevelManager.MyStartPosition = transform.position;
            myPlayerHasEnteredCheckpoint = true;
        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(transform.position.x, -500), new Vector3(transform.position.x, 500));
        Gizmos.color = new Color(255, 128, 0);
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + myRange, transform.position.y));
        Gizmos.DrawLine(new Vector3(transform.position.x + myRange, transform.position.y - 10), new Vector3(transform.position.x + myRange, transform.position.y + 10));
    }

    void CheckPointColideCheck()
    {
        //RaycastHit2D hitinfo = Physics2D.BoxCast(transform.position, transform.localScale + Vector3.one, 0, Vector3.up, 0, myLayerMask);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.CompareTag("Player"))
    //    { 
    //        CheckPointManager.myInstance.myLastCheckPoint = transform;

    //    }
    //}
}
