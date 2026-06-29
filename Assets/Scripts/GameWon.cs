using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWon : MonoBehaviour
{
    public string menuSceneName = "MainMenu";

    public SceneFader sceneFader;

    public string nextLevel = "Level02";
    public int levelToReached = 2;
    
    public StarRatingUI starUI;
    
    public SupabaseManager supabase;

    private GameManager gm;
    
    private void OnEnable()
    {
        ShowStarsOnOpen();
        gm = FindFirstObjectByType<GameManager>();
    }
    
    public void Continue()
    {
        StartCoroutine(SaveAndContinue());
    }

    public void Menu()
    {
        StartCoroutine(SaveAndGoMenu());
    }
    
    IEnumerator SaveAndContinue()
    {
        SendLevelCompletedAnalytics();
        
        yield return supabase.UpdateLevelReached(levelToReached-1);
        
        if (levelToReached > PlayerPrefs.GetInt("LevelReached", 2))
        {
            PlayerPrefs.SetInt("LevelReached", levelToReached);
            PlayerPrefs.Save();
        }
        
        SaveStars();
        
        yield return PlayerProgressManager.instance.SaveOnGameOver();

        sceneFader.FadeTo(nextLevel);
    }
    
    IEnumerator SaveAndGoMenu()
    {
        SendLevelCompletedAnalytics();
        
        yield return supabase.UpdateLevelReached(levelToReached-1);
        
        if (levelToReached > PlayerPrefs.GetInt("LevelReached", 2))
        {
            PlayerPrefs.SetInt("LevelReached", levelToReached);
            PlayerPrefs.Save();
        }
        
        SaveStars();
        
        yield return PlayerProgressManager.instance.SaveOnGameOver();

        sceneFader.FadeTo(menuSceneName);
    }
    
    void ShowStarsOnOpen()
    {
        gm = FindFirstObjectByType<GameManager>();
        StarRating rating = gm.CalculateStars();

        starUI.Show(rating);
    }
    
    void SaveStars()
    {
        string levelName = SceneManager.GetActiveScene().name;

        StarRating rating = gm.CalculateStars();

        int starsEarned = 0;

        switch (rating)
        {
            case StarRating.Bronze:
                starsEarned = 1;
                break;
            case StarRating.Silver:
                starsEarned = 2;
                break;
            case StarRating.Gold:
                starsEarned = 3;
                break;
        }

        int oldStars = PlayerPrefs.GetInt(levelName + "Stars", 0);

        if (starsEarned > oldStars)
        {
            PlayerPrefs.SetInt(levelName + "Stars", starsEarned);
            PlayerPrefs.Save();
        }
    }
    
    void SendLevelCompletedAnalytics()
    {
        if (AnalyticsManager.Instance == null)
            return;

        AnalyticsManager.Instance.LevelCompleted(
            SceneManager.GetActiveScene().name,
            (int)FindFirstObjectByType<GameManager>().CalculateStars()
        );
    }
}