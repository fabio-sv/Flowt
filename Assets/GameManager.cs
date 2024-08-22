using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject throwable;
    [SerializeField] GameObject attractor;

    public GameObject gameOverScreen;
    public GameObject gameWonScreen;
    public GameObject suggestionsCanvas;
    public TMP_Text gameTextBox;

    private GameObject planet;
    private List<GameObject> blackholes = new();

    List<GameObject> active = new();


    int activeLevelIndex = 0;
    private List<Level> Levels = new List<Level>()
    {
        new(5, new Vector3(1, 0, 0), new List<Vector3> { new(0, 0, 0) }),
        new(10, new Vector3(0, 0, 0), new List<Vector3> { new(-1, 0, 0), new(1, 0, 0) }),
        new(10, new Vector3(0, 0, 0), new List<Vector3> { new(-1, 0.5f, 0), new(-1, -0.5f, 0), new Vector3(1, 0, 0)}),
    };

    private bool gameIsOver = false;
    private bool gameIsWon = false;
    public float xBound = 4f;
    public float yBound = 2.5f;
    private int interactions = 0;

    void Start()
    {
        Debug.Log($"Loading level ${activeLevelIndex}...");
        LoadLevel(activeLevelIndex);
    }

    void Update()
    {
        if (gameIsOver || gameIsWon)
        {
            return;
        }

        if (userHasWon())
        {
            gameWon();
            return;
        }

        if (isOutOfBounds())
        {
            gameOver();
            return;
        }
    }

    // To fix or remove
    public void startGame()
    {
        const string GAME_SCENE = "Level";
        SceneManager.LoadScene(GAME_SCENE);
    }

    public void gameOver()
    {
        gameIsOver = true;
        
        getThrowable().Pause();
        gameOverScreen.SetActive(true);

        TMP_Text scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();

        scoreText.text = $"t = {getThrowable().getTimeAlive().ToString()}s";
    }

    private void gameWon()
    {
        gameIsWon = true;

        gameWonScreen.SetActive(true);

        if (!hasNextLevel())
        {
            GameObject nextButton = GameObject.Find("NextLevelButton");
            nextButton.SetActive(false);

        }
    }

    public void interacted()
    {
        interactions++;

        Debug.Log(suggestionsCanvas);

        if (interactions > 0 && suggestionsCanvas.activeInHierarchy)
        {
            suggestionsCanvas.SetActive(false);
        }
    }

    private void LoadLevel(int levelNumber)
    {

        if (levelNumber > Levels.Count)
        {
            throw new Exception($"issues ${levelNumber} ${Levels.Count}");
        }

        Level levelToLoad = Levels[levelNumber];
        gameTextBox.text = $"Goal: {levelToLoad.GoalTime.ToString()}s";

        Debug.Log(throwable);

        // Planet
        planet = Instantiate(throwable, levelToLoad.ToyStartPosition, Quaternion.identity);
        planet.name = "Planet";

        active.Add(planet);

        // Blackholes
        foreach (Vector3 attractorPosition in levelToLoad.AttractorPositions)
        {
            GameObject bh = Instantiate(attractor, attractorPosition, Quaternion.identity);
            bh.name = "Attractor";

            blackholes.Add(bh);
            active.Add(bh);
        }
    }

    private Throwable getThrowable()
    {
        return planet.GetComponent<Throwable>();
    }

    private bool isOutOfBounds()
    {
        Vector3 position = getThrowable().transform.position;

        return Mathf.Abs(position.x) > xBound || Mathf.Abs(position.y) > yBound;
    }

    private bool userHasWon()
    {
        int timeAlive = getThrowable().getTimeAlive();

        int goalTime = Levels[activeLevelIndex].GoalTime;

        return timeAlive >= goalTime;
    }

    private bool hasNextLevel()
    {

        Debug.Log($"hasNextLevel {activeLevelIndex} {Levels.Count} {activeLevelIndex + 1 < Levels.Count}");
        return activeLevelIndex + 1 < Levels.Count;
    }

    private void Reset()
    {
        foreach (GameObject obj in active)
        {
            Destroy(obj);
        }

        gameIsOver = false;
        gameIsWon = false;
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);

        Debug.Log("reset complete");
    }

    public void LoadNextLevel()
    {
        Debug.Log("resetting...");
        Reset();
        activeLevelIndex++;
        LoadLevel(activeLevelIndex);
        Debug.Log($"new index should be {activeLevelIndex}");
    }
}
