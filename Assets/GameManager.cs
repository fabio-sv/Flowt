using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameObject planet;
    public GameObject gameOverScreen;

    public float spawnRate = 1;
    public bool enableSpawn = true;
    private float timer = 0;

    public CanvasGroup fadingCanvasGroup;
    private bool isFaded = false;

    private int interactions = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!enableSpawn)
        //{
        //    return;
        //}

        //if (timer < spawnRate)
        //{
        //    timer += Time.deltaTime;
        //}
        //else
        //{
        //    Vector3 v3 = new Vector3(randomNumber(), randomNumber(), 0);
        //    Vector3 normal = getOrthogonal(v3);

        //    GameObject obj = Instantiate(planet, v3, Quaternion.identity);

        //    Graviton graviton = obj.GetComponent<Graviton>();
        //    graviton.initialVelocity = normal;
        //    graviton.applyInitialVelocityOnStart = true;

        //    SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        //    sprite.color = randomColor();

        //    TrailRenderer trail = obj.GetComponentInChildren<TrailRenderer>(true);
        //    trail.enabled = true;

        //    // TrailRenderer trail = obj.GetComponent<TrailRenderer();


        //    timer = 0;
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Debug.Log($"mouse down: {mousePos.ToString()}");

        //    transform.position = mousePos;
        //    Instantiate(planet, transform);
        //}
    }

    float randomNumber()
    {
        return ((float)Random.Range(-10, 10)) / 10.0f;
    }

    Color randomColor()
    {
        //return new Color(randomNumber(), randomNumber(), randomNumber());

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
        fader();
        gameOverScreen.SetActive(true);
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
