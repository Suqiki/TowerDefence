using TMPro;
using UnityEngine;
using DG.Tweening;

public class WarningUI : MonoBehaviour
{
    public static WarningUI instance;

    public TextMeshProUGUI warningText;

    private void Awake()
    {
        instance = this;
    }

    public void ShowWarning(string message)
    {
        if (warningText == null) return;

        // Oprim orice animație DOTween activă direct pe componentă sau pe text
        DOTween.Kill(warningText);
    
        warningText.gameObject.SetActive(true);
        warningText.alpha = 1f;
        warningText.text = message;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        
        // Înlocuim DOFade cu o metodă generică DOTween.To pentru a modifica alpha-ul textului
        seq.Append(DOTween.To(() => warningText.alpha, x => warningText.alpha = x, 0f, 0.5f));
        
        seq.OnComplete(() =>
        {
            if (warningText != null)
            {
                warningText.gameObject.SetActive(false);
            }
        });
    }
}