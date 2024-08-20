using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool gameIsOver = false;
    public GameObject planet;
    public GameObject gameOverScreen;

    public float spawnRate = 1;
    public bool enableSpawn = true;
    private float timer = 0;

    public CanvasGroup fadingCanvasGroup;
    private bool isFaded = false;

    private int interactions = 0;

    private Throwable toy;

    public float xBound = 4f;
    public float yBound = 2.5f;

    public void fader()
    {
        isFaded = !isFaded;

        if (isFaded)
        {
            fadingCanvasGroup.DOFade(1, 2);
        }
        else
        {
            fadingCanvasGroup.DOFade(0, 2);
        }
    }

    void Start()
    {
        toy = GameObject.Find("Throwable").GetComponent<Throwable>();
    }

    void Update()
    {
        if (gameIsOver)
        {
            return;
        }

        Vector3 position = toy.transform.position;

        if (Mathf.Abs(position.x) > xBound || Mathf.Abs(position.y) > yBound)
        {
            this.gameOver();
        }
    }

    float randomNumber()
    {
        return ((float)Random.Range(-10, 10)) / 10.0f;
    }

    Color randomColor()
    {
        Color[] colors = {
            new Color(165f / 255f, 214f / 255f, 175f / 255f),
            new Color(166f / 255f, 145f / 255f, 219f/ 255f),
            new Color(245f / 255f, 145f / 255f, 205f / 255f),
            new Color(255f / 255f, 200f / 255f, 148f / 255f),
            new Color(255f / 255f, 245f / 255f, 189f / 255f),
        };

        return colors[Random.Range(0, colors.Length)];
    }

    Vector3 getOrthogonal(Vector3 input)
    {
        return new Vector3(-input.y * 2, input.x * 2, 0);
    }

    public void startGame()
    {
        const string GAME_SCENE = "Gravity";
        SceneManager.LoadScene(GAME_SCENE);
    }

    public void gameOver()
    {
        gameIsOver = true;
        toy.Pause();
        gameOverScreen.SetActive(true);

        TMP_Text scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();

        scoreText.text = $"t = {toy.getTimeAlive().ToString()}s";
    }

    public void interacted()
    {
        interactions++;
        GameObject suggestionsCanvas = GameObject.Find("SuggestionsCanvas");

        if (interactions > 0 && suggestionsCanvas.activeInHierarchy)
        {
            suggestionsCanvas.SetActive(false);
        }
    }
}
