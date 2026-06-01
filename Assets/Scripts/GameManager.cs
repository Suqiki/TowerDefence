using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.LowLevelPhysics2D;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver;

    public GameObject gameUI;
    public GameObject gameOverUI;
    public GameObject gameWonUI;
    
    [Header("Sounds")]
    public AudioClip[] gameOverSound;
    public float gameOverVolume=1f;
    public AudioClip gameWonSound;
    public float gameWonVolume=1f;
    
    private void Start()
    {
        if (PlayerProgressManager.instance != null)
        {
            PlayerProgressManager.instance.ResetLevelStats();
        }
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
        if (SoundEffectsManager.instance != null && gameOverSound != null)
        {
            SoundEffectsManager.instance.PlayrandomSoundEffect(gameOverSound, transform, gameOverVolume);
        }
        gameIsOver = true;
        //Debug.Log("Game End");
        //gameUI.SetActive(false);
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        
    }

    public void WinLevel()
    {
        if (SoundEffectsManager.instance != null && gameWonSound != null)
        {
            SoundEffectsManager.instance.PlaySoundEffect(gameWonSound, transform, gameWonVolume);
        }
        gameIsOver = true;
        if (gameWonUI != null)
        {
            gameWonUI.SetActive(true);
        }
    }
    
    public StarRating CalculateStars()
    {
        int lives = PlayerStats.Lives;

        if (lives >= 8)
            return StarRating.Gold;

        if (lives >= 5)
            return StarRating.Silver;

        if (lives >= 1)
            return StarRating.Bronze;

        return StarRating.None;
    }
}


public enum StarRating
{
    None,
    Bronze,
    Silver,
    Gold
}
