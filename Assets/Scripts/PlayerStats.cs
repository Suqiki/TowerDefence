using TMPro;
using UnityEngine;
using DG.Tweening;

public class PlayerStats : MonoBehaviour
{
    public static int Gold;
    public int startGold = 400;

    public static int Lives;
    public int startLives = 10;

    public TextMeshProUGUI goldNrText;

    private Color goldTextColorOrigin;
    public Color colorMinus;

    private int lastGold;

    [HideInInspector]
    public static int rounds;

    void Start()
    {
        rounds = 0;
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
        // Oprim orice animație activă pe această componentă text
        DOTween.Kill(goldNrText);

        var sequence = DOTween.Sequence();
    
        // Animăm valoarea culorii direct prin proprietatea .color, pas cu pas
        sequence.Append(DOTween.To(() => goldNrText.color, x => goldNrText.color = x, colorMinus, 0.3f));
        sequence.Append(DOTween.To(() => goldNrText.color, x => goldNrText.color = x, goldTextColorOrigin, 0.3f));
    }
    
}