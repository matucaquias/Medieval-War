using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    private Rigidbody2D _rb;
    public Vector3 pointA = new Vector3(3, 0, 0);
    public Vector3 pointB = new Vector3(-3, 0, 0);
    public float speed;
    public float movementTime=3;
    private float t;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        t += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = Vector3.Lerp(pointA, pointB, t * speed);
        _rb.velocity = new Vector2(targetVelocity.x,0);
        if (t*speed>=speed*movementTime)
        {
            Vector3 b = pointB;
            Vector3 a = pointA;
            pointA = b;
            pointB = a;
            t = 0;
        }
    }
}
