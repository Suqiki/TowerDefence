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
        warningText.DOKill();
    
        warningText.gameObject.SetActive(true);
        warningText.alpha = 1;
        warningText.text = message;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.Append(warningText.DOFade(0, 0.5f));
        seq.OnComplete(() =>
        {
            warningText.gameObject.SetActive(false);
        });
    }
}