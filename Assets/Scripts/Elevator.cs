using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float targetHeight;
    private Rigidbody2D _rb;
    private Vector3 _startPos;
    private float t;
    public float speed;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPos = transform.localPosition;
    }
    
    private void Update()
    {
        t += Time.deltaTime;
    }
    
    private void FixedUpdate()
    {
        if (transform.localPosition.y > targetHeight)
        {
            Vector3 targetVelocity = Vector3.Lerp(_startPos, new Vector2(_startPos.x,targetHeight), t * speed);
            _rb.velocity = new Vector2(0,targetVelocity.y);
        }else if (transform.localPosition.y <= targetHeight)
        {
            _rb.bodyType = RigidbodyType2D.Static;
        }
    }
}
