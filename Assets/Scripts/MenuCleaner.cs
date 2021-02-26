using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCleaner : MonoBehaviour
{
    private GameObject[] _toClean;
    private DifficultyManager _difficultyManager;
    void Start()
    {
        getObjects();
        if (_toClean.Length > 1)
        {
            Destroy(_toClean[1]);
        }
        _difficultyManager = GameObject.FindGameObjectWithTag("DifficultyManager").GetComponent<DifficultyManager>();
        _difficultyManager.menuMusic.UnPause();
    }

    GameObject[] getObjects()
    {
        DifficultyManager[] enemies = FindObjectsOfType<DifficultyManager>();
        GameObject[] objects = new GameObject[enemies.Length];
        for (int i = 0; i < objects.Length; i++)
            objects[i] = enemies[i].gameObject;
        _toClean = objects;
        return objects;
    }
}
