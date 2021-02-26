using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    private CharacterController2D _controller;

    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
    }


    private void FixedUpdate()
    {
        if (_controller != null)
        {
            _controller.Pursuit();
            _controller.EnemyMove();
        }
    }
    

}
