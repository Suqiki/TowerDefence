using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public GameObject ui;
    
    public string menuSceneName="MainMenu";
    
    public SceneFader sceneFader;
    
    public static bool isPaused = false;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);
        
        isPaused = ui.activeSelf;

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            if (TutorialManager.instance != null &&
                TutorialManager.instance.tutorialEnabled &&
                TutorialManager.instance.IsTutorialPaused())
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void Retry()
    {
        Toggle();
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Toggle();
        sceneFader.FadeTo(menuSceneName);
    }
}
