using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver;

    public GameObject gameUI;
    public GameObject gameOverUI;


    private void Start()
    {
        gameIsOver = false;
       // gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (gameIsOver)
            return;
        
        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameIsOver = true;
        //Debug.Log("Game End");
        //gameUI.SetActive(false);
        gameOverUI.SetActive(true);
    }
    
}
