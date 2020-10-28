﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{

    [SerializeField]
    [Tooltip("THIS WILL BE AUTOMATICALLY FILLED")]
    private Player myPlayer;
    [SerializeField]
    [Tooltip("THIS WILL BE AUTOMATICALLY FILLED")]
    private LevelManager myLevelManager;

    bool myLevelEnded = false;

    // Start is called before the first frame update
    void OnValidate()
    {
        myPlayer = FindObjectOfType<Player>();
        myLevelManager = FindObjectOfType<LevelManager>();

    }

    private void Start()
    {
        myLevelEnded = false;

    }
    // Update is called once per frame
    void Update()
    {

        if(myPlayer.transform.position.x > transform.position.x && myLevelEnded == false)
        {
            myLevelEnded = true;
            myLevelManager.LevelComplete();

        }

    }
}
