using TMPro;
using UnityEngine;
using DG.Tweening;

public class PlayerStats : MonoBehaviour
{
    public static int Gold;
    public int startGold = 400;

    public static int Lives;
    public int startLives = 20;

    public TextMeshProUGUI goldNrText;

    private Color goldTextColorOrigin;
    public Color colorMinus;

    private int lastGold;

    void Start()
    {
        Lives=startLives;
        Gold = startGold;
        lastGold = Gold;
        goldTextColorOrigin = goldNrText.color;
    }

    void Update()
    {
        goldNrText.text = " " + Gold;

        if (Gold < lastGold)
        {
            MinusGold();
        }

        lastGold = Gold;
    }

    public void MinusGold()
    {
        goldNrText.DOKill();

        var sequence = DOTween.Sequence();
        sequence.Append(goldNrText.DOColor(colorMinus, 0.3f));
        sequence.Append(goldNrText.DOColor(goldTextColorOrigin, 0.3f));
    }
    
}