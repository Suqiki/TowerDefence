using UnityEngine;

public class StarRatingUI : MonoBehaviour
{
    public GameObject bronzeStar;
    public GameObject silverStar;
    public GameObject goldStar;

    public void Show(StarRating rating)
    {
        bronzeStar.SetActive(false);
        silverStar.SetActive(false);
        goldStar.SetActive(false);

        switch (rating)
        {
            case StarRating.Bronze:
                bronzeStar.SetActive(true);
                break;

            case StarRating.Silver:
                silverStar.SetActive(true);
                break;

            case StarRating.Gold:
                goldStar.SetActive(true);
                break;
        }
    }
}