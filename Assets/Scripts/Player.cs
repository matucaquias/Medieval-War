using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController2D _controller;
    private float _horizontalMove = 0f;
    private bool _jump = false;
    private Animator _animator;
    public AudioSource footsteps;
    public AudioSource bite;
    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_controller != null)
        {
            _horizontalMove = Input.GetAxisRaw("Horizontal") * _controller.speed;
            if (_horizontalMove == 0)
            {
                footsteps.Pause();
            }else footsteps.UnPause();

            if (!_jump)
            {
                _animator.SetFloat("xMove",_horizontalMove);
            }

            if (Input.GetButtonDown("Jump"))
            {
                _jump = true;
                _controller.Jump(_jump);
                StartCoroutine(WaitFunc(1f));
                _jump = false;
            }

            if (Input.GetButtonDown("Attack"))
            {
                _controller.Attack();
            }

            if (Input.GetButtonDown("SecondaryAttack"))
            {
                _controller.SecondaryAttack();
            }
        }

    }


    IEnumerator WaitFunc(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private void FixedUpdate()
    {
        _controller.Move(_horizontalMove * Time.fixedDeltaTime);
    }
}
