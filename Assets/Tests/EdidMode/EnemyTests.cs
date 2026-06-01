using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTests
{
    private GameObject enemyObject;
    private Enemy enemy;
    private GameObject dummyPrefab;

    [SetUp]
    public void Setup()
    {
        enemyObject = new GameObject();
        enemy = enemyObject.AddComponent<Enemy>();

        enemy.startHealth = 100f;
        enemy.startSpeed = 10f;

        // 1. REPARARE TAKE DAMAGE: Îi atașăm o componentă Image falsă pentru bara de viață
        GameObject canvasObj = new GameObject("DummyCanvas");
        Image dummyImage = canvasObj.AddComponent<Image>();
        enemy.healthBar = dummyImage;

        // 2. REPARARE DOT EFFECT: Îi dăm un obiect gol ca să aibă ce să instanțieze la ApplyDoT
        dummyPrefab = new GameObject("DummyPrefab");
        enemy.dotEffectPrefab = dummyPrefab;

        // Apelăm metoda de inițializare din script
        enemy.Initializehealth();
        enemy.speed = enemy.startSpeed;
    }

    [TearDown]
    public void Teardown()
    {
        // Curățăm absolut toate obiectele create în timpul testului
        Object.DestroyImmediate(enemyObject);
        if (dummyPrefab != null)
        {
            Object.DestroyImmediate(dummyPrefab);
        }
        
        // Ștergem și canvas-ul temporar rămas în scenă
        GameObject canvas = GameObject.Find("DummyCanvas");
        if (canvas != null)
        {
            Object.DestroyImmediate(canvas);
        }
    }

    [Test]
    public void Enemy_TakesDamage()
    {
        enemy.TakeDamage(20f);

        // Inamicul nu ar trebui să fie marcat ca mort la doar 20 damage
        Assert.IsFalse(enemy.isDead);
        
        // OPȚIONAL: Verificăm dacă bara de viață s-a calculat corect (100 - 20 = 80 -> 0.8f)
        Assert.AreEqual(0.8f, enemy.healthBar.fillAmount, 0.001f);
    }

    [Test]
    public void Enemy_Dies_WhenHealthBelowZero()
    {
        // Aplicăm un damage fatal (peste 100)
        enemy.TakeDamage(120f);

        // Verificăm dacă proprietatea isDead a devenit true
        Assert.IsTrue(enemy.isDead);
    }

    [Test]
    public void Enemy_Slow_ReducesSpeed()
    {
        enemy.Slow(0.5f);

        Assert.AreEqual(5f, enemy.speed);
    }

    [Test]
    public void Enemy_DoT_IsApplied()
    {
        enemy.ApplyDoT(10f, 5f);

        Assert.AreEqual(10f, enemy.DoT);
        Assert.AreEqual(5f, enemy.dotDuration);
    }
}