using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public int enemiesLeft;
    public GameObject winConditionText;
    public GameObject chest;
    public GameObject newPerk;
    private CharacterController2D _player;
    private CharacterController2D _boss;
    private DifficultyManager _difficultyManager;

    public GameObject key1;
    public GameObject key2;

    public bool keysCollected;

    public GameObject[] level3Platforms;
    private LOADER _loader;

    //Cheats
    private bool _cheatsEnabled;
    public GameObject[] enemyObjects;
    
    
    void Awake()
    {
        getObjects();
        _difficultyManager = GameObject.FindGameObjectWithTag("DifficultyManager").GetComponent<DifficultyManager>();
        _difficultyManager.SetDifficulty(enemyObjects);
        _difficultyManager.menuMusic.Pause();
        _cheatsEnabled = _difficultyManager.cheatsEnabled;
    }

    private void Start()
    {
        _loader = GameObject.FindGameObjectWithTag("LOADER").GetComponent<LOADER>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        if (SceneManager.GetActiveScene().name == "Level 3")
        {
            _player.Grow();
            _player.appleCollected = true;
            _player.growAppleCollected = true;
            _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<CharacterController2D>();
        }
        if (SceneManager.GetActiveScene().name == "Level 2")
        {
            _player.appleCollected = true;
        }
        
    }

    void Update()
    {
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (_cheatsEnabled)
        {
            getObjects();
            Cheats();
        }

        if (_player == null)
        {
            _loader.LoadLevel("Lose");
        }
        
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            if (enemiesLeft == 0)
            {
                winConditionText.SetActive(true);
                chest.SetActive(true);
            }

            if (_player != null)
            {
                if (_player.appleCollected)
                {
                    newPerk.SetActive(true);
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Level 2")
        {

            if (enemiesLeft == 0 && keysCollected)
            {
                winConditionText.SetActive(true);
                chest.SetActive(true);
            }
            
            if (_player != null)
            {
                if (_player.growAppleCollected)
                {
                    newPerk.SetActive(true);
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Level 3" && enemiesLeft == 0 && _boss.attackCount == 3)
        {
            foreach (var platform in level3Platforms)
            {
                platform.SetActive(true);
            }

            if (_boss == null)
            {
                _loader.LoadLevel("Win");
            }
        }
    }

    private void Cheats()
    {
        if (Input.GetKeyDown(KeyCode.F1) && enemyObjects.Length != 0)
        {
            foreach (var enemy in enemyObjects)
            {
                enemy.GetComponent<CharacterController2D>().life = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (_player.life < _player.maxLife)
            {
                _player.life += 10;
            }
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            _player.damage = 50;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            _player.jumpForce = 1000;
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            _player.attackCooldown = 0.3f;
        }
    }

    public void EnableCheats()
    {
        _cheatsEnabled = true;
    }

    GameObject[] getObjects()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        GameObject[] objects = new GameObject[enemies.Length];
        for (int i = 0; i < objects.Length; i++)
            objects[i] = enemies[i].gameObject;
        enemyObjects = objects;
        return objects;
    }

}
