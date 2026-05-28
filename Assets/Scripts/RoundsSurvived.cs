using System.Collections;
using TMPro;
using UnityEngine;

public class RoundsSurvived : MonoBehaviour
{
    public TextMeshProUGUI roundText;

    void OnEnable()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        roundText.text = "Rounds: 0";
        int round = 0;
        
        yield return new WaitForSeconds(.7f);
        
        while (round < PlayerStats.rounds)
        {
            round++;
            roundText.text = "Rounds: " + round.ToString();

            yield return new WaitForSeconds(0.05f);
        }
    }
}
