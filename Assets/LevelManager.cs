using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject throwable;
    [SerializeField] GameObject attractor;

    private List<Level> Levels = new List<Level>()
    {
        //new(20, new Vector3(0, 0, 0), [new Vector3(-1, 0, 0), new Vector3(1, 0, 0)])
    };


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevel(int levelNumber)
    {
        if (levelNumber > Levels.Count)
        {
            throw new Exception($"issues ${levelNumber} ${Levels.Count}");
        }

        Level levelToLoad = Levels[levelNumber];

        SceneManager.LoadScene("Gravity");

        // Throwable
        Instantiate(throwable, levelToLoad.ToyStartPosition, Quaternion.identity);

        foreach (Vector3 attractorPosition in levelToLoad.AttractorPositions)
        {
            Instantiate(attractor, attractorPosition, Quaternion.identity);
        }
    }
}
