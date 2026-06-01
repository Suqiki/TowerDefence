using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    public SceneFader sceneFader;

    public string menuSceneName = "MainMenu";
    
    
    private void Start()
    {
        // automat la spawn UI GameOver
        StartCoroutine(SaveAndShowGameOver());
    }

    IEnumerator SaveAndShowGameOver()
    {
        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.LevelFailed(
                SceneManager.GetActiveScene().name
            );
        }
        
        yield return PlayerProgressManager.instance.SaveOnGameOver();

        // după save arată UI / sau rămâi pe ecran
        Debug.Log("Game Over stats saved");
    }
    public void Retry()
    {
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        sceneFader.FadeTo(menuSceneName);
    }
}
