using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private GameMaster _gm;
    void Start()
    {
        _gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            Destroy(gameObject);
            if (gameObject.name == "Key1")
            {
                _gm.key1.SetActive(true);
            }else if (gameObject.name == "Key2")
            {
                _gm.key2.SetActive(true);
                _gm.keysCollected = true;
            }
        }
    }
}
