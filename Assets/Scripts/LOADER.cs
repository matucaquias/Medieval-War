using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LOADER : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.5f;
    private DifficultyManager _difficultyManager;
    public GameObject cheatsOn;
    public GameObject cheatsOff;
    public AudioSource clickSound;
    
    private void Awake()
    {
        _difficultyManager = GameObject.FindGameObjectWithTag("DifficultyManager").GetComponent<DifficultyManager>();
    }

    public void LoadLevel(string scene)
    {
        StartCoroutine(Load(scene));
    }

    public void Restart()
    {
        StartCoroutine(Load(SceneManager.GetActiveScene().name));
    }

    IEnumerator Load(string scene)
    {
        transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(transitionTime);
        
        SceneManager.LoadScene(scene);
    }

    public void ChangeCheats()
    { 
        _difficultyManager.SetCheats();
    }

    public void CheatsEnabled()
    {
        cheatsOff.SetActive(true);
        cheatsOn.SetActive(false);
    }

    public void CheatsDisabled()
    {
        cheatsOff.SetActive(false);
        cheatsOn.SetActive(true);
    }


    public void SetDifficulty(int difficulty)
    {
        _difficultyManager.difficulty = difficulty;
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
