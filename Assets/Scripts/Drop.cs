using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public float lifeDrop = 15;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            var player = other.gameObject.GetComponent<CharacterController2D>();
            Destroy(gameObject);
            if (player.life < player.maxLife)
            {
                player.life += lifeDrop;
            }
        }
    }
}
