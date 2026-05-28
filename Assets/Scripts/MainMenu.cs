using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "SampleScene";
    public SceneFader sceneFader;
    public GameObject settingsUI;
    public void Play()
    {
        sceneFader.FadeTo(levelToLoad);
    }

    public void Settings()
    {
        settingsUI.SetActive(true);
    }
    
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
