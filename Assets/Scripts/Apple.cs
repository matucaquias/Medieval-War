using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Apple : MonoBehaviour
{
    private Rigidbody2D _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(new Vector2(0,400));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<CharacterController2D>();
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            if (player.GetComponent<Player>() != null)
            {
                Destroy(gameObject);
                player.EnableSecondaryAttack();
                player.GetComponent<Player>().bite.Play();
            }
        }

        if (SceneManager.GetActiveScene().name == "Level 2")
        {
            if (player.GetComponent<Player>() != null)
            {
                Destroy(gameObject);
                player.growAppleCollected = true;
                player.Grow();
                player.GetComponent<Player>().bite.Play();
            }
        }
    }
}
