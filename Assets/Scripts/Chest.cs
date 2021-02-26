using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator _animator;
    public GameObject apple;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.GetComponent<Player>() != null)
        {
            _animator.SetTrigger("Open");
            gameObject.layer = 12;
            Instantiate(apple, transform.position, transform.rotation);
        }
    }
}
