using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("started...");
    }

    public void startGame()
    {
        Debug.Log("loading Gravity scene");

        const string GAME_SCENE = "Level";
        SceneManager.LoadScene(GAME_SCENE);
    }
}
