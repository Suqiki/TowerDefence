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
    
    private void OnEnable()
    {
        ShowStarsOnOpen();
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
        
        yield return PlayerProgressManager.instance.SaveOnGameOver();

        sceneFader.FadeTo(menuSceneName);
    }
    
    void ShowStarsOnOpen()
    {
        GameManager gm = FindFirstObjectByType<GameManager>();
        StarRating rating = gm.CalculateStars();

        starUI.Show(rating);
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