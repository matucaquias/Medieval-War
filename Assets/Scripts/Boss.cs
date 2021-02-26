using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private CharacterController2D _controller;
    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        StartCoroutine(WaitFunc(4f));
    }

    private void Update()
    {
        if (_controller.attackCount == 3 && _controller != null)
        {
            _controller.isRanged = true;
            _controller.EnemyMove();
            _controller.SecondaryAttack();
        }
    }

    IEnumerator WaitFunc(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartCoroutine(_controller.Boss());
    }
}
