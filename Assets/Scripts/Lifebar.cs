using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{
    public CharacterController2D controller;
    public Image lifebar;
    // Update is called once per frame
    void Update()
    {
        if (controller != null)
        {
            lifebar.fillAmount = controller.life / controller.maxLife;
        }
    }
}
