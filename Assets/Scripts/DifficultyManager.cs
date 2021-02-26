using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public int difficulty = 1;
    public bool cheatsEnabled = false;
    public AudioSource menuMusic;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public void SetDifficulty(GameObject[] enemies)
    {
        if (difficulty == 0)
        {
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<CharacterController2D>().maxLife *= .75f;
                enemy.GetComponent<CharacterController2D>().damage *= .75f;
                enemy.GetComponent<CharacterController2D>().lookRadius *= .75f;
            }
        }
        else if (difficulty == 2)
        {
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<CharacterController2D>().maxLife *= 1.3f;
                enemy.GetComponent<CharacterController2D>().damage *= 1.3f;
                enemy.GetComponent<CharacterController2D>().lookRadius *= 1.3f;
            }
        }
    }

    public void SetCheats()
    {
        cheatsEnabled = !cheatsEnabled;
    }
}
