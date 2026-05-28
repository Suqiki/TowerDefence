using UnityEngine;

public class LevelButtonUI : MonoBehaviour
{
    public string levelName = "Level01";

    public GameObject blackStar;
    public GameObject bronzeStar;
    public GameObject silverStar;
    public GameObject goldStar;

    private void Start()
    {
        int stars = PlayerPrefs.GetInt(levelName + "Stars", 0);

        bronzeStar.SetActive(false);
        silverStar.SetActive(false);
        goldStar.SetActive(false);

        switch (stars)
        {
            case 1:
                blackStar.SetActive(false);
                bronzeStar.SetActive(true);
                break;

            case 2:
                blackStar.SetActive(false);
                silverStar.SetActive(true);
                break;

            case 3:
                blackStar.SetActive(false);
                goldStar.SetActive(true);
                break;
        }
    }
}