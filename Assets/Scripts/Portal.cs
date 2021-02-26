using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private GameMaster _gm;
    private LOADER _loader;
    private void Start()
    {
        _gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
        _loader = GameObject.FindGameObjectWithTag("LOADER").GetComponent<LOADER>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            if (player != null && _gm.enemiesLeft == 0)
            {
                _loader.LoadLevel("Level 2");
            }
        }else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            if (player != null && _gm.enemiesLeft == 0 && _gm.keysCollected)
            {
                _loader.LoadLevel("Level 3");
            }
        }
    }
}
