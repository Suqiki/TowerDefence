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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        enemyTextColorOrigin = enemyText.color;
        enemiesAlive = 0;
        Refresh();
    }

    public void Refresh()
    {
        enemyText.text = "Enemies Alive: " + enemiesAlive;
    }

    public void MinusEnemy()
    {
        enemiesAlive--;

        if (enemiesAlive < 0)
        {
            enemiesAlive = 0;
            Debug.LogWarning("EnemiesAlive went below zero!");
        }
        
        Refresh();

        var sequence = DOTween.Sequence();
        sequence.Append(enemyText.DOColor(colorMinus, 0.3f));
        sequence.Append(enemyText.DOColor(enemyTextColorOrigin, 0.3f));
    }
    
    public void PlusEnemy()
    {
        enemiesAlive++;
        
        Refresh();
        
        var sequence = DOTween.Sequence();
        sequence.Append(enemyText.DOColor(colorPlus, 0.3f));
        sequence.Append(enemyText.DOColor(enemyTextColorOrigin,  0.3f));
    }
}
