using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed;
    public float damage;
    private Rigidbody2D _rb;
    public Transform attackPoint;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = speed * attackPoint.right;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<CharacterController2D>() != null)
        {
            Destroy(gameObject);
            other.gameObject.GetComponent<CharacterController2D>().TakeDamage(damage);
        }else if (other.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
}
