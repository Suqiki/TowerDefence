using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;
    
    public Button[] LevelButtons;

    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("LevelReached", 2);
        
        for (int i = 0; i < LevelButtons.Length; i++)
        {
            if(i+1>levelReached)
                LevelButtons[i].interactable = false;
        }
    }

    public void Select(string levelName)
    {
        fader.FadeTo(levelName);
    }

    public void Return()
    {
        fader.FadeTo("MainMenu");
    }
}
