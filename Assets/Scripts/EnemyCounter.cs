using System;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    public static int enemiesAlive = 0;

    public TextMeshProUGUI enemyText;

    private Color enemyTextColorOrigin;
    public Color colorMinus;
    public Color colorPlus;

    public static EnemyCounter instance;
    
    public TextMeshProUGUI EnemyText
    {
        get => enemyText;
        set => enemyText = value;
    }

    private void Awake()
    {
        instance = this;
        enemyTextColorOrigin = enemyText.color;
    }

    void Update()
    {
        enemyText.text = "Enemies Alive: " + enemiesAlive;
    }

    public void MinusEnemy()
    {
        enemiesAlive--;
        
        var sequence = DOTween.Sequence();
        sequence.Append(enemyText.DOColor(colorMinus, 0.3f));
        sequence.Append(enemyText.DOColor(enemyTextColorOrigin,  0.3f));
    }
    
    public void PlusEnemy()
    {
        enemiesAlive++;
        
        var sequence = DOTween.Sequence();
        sequence.Append(enemyText.DOColor(colorPlus, 0.3f));
        sequence.Append(enemyText.DOColor(enemyTextColorOrigin,  0.3f));
    }
}
