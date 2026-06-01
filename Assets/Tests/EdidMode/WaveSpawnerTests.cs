using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class WaveSpawnerTests
{
    private GameObject spawnerObject;
    private WaveSpawner waveSpawner;
    private GameObject dummyManagerObj;

    [SetUp]
    public void Setup()
    {
        spawnerObject = new GameObject("WaveSpawner");
        waveSpawner = spawnerObject.AddComponent<WaveSpawner>();

        // 1. Simulăm Textul UI separat ca să nu dea erori de duplicare componentă
        GameObject textObj = new GameObject("WaveTextObj");
        waveSpawner.waveText = textObj.AddComponent<TextMeshProUGUI>();

        // 2. Simulăm GameManager pentru a intercepta starea de Win
        GameObject gmObj = new GameObject("GameManagerObj");
        waveSpawner.gameManager = gmObj.AddComponent<GameManager>();

        // 3. Înlocuim punctul de spawn cu un transform dummy
        GameObject spawnPointObj = new GameObject("SpawnPointObj");
        waveSpawner.spawnPoint = spawnPointObj.transform;

        // 4. Inițializăm un vector de valuri gol (0 valuri)
        waveSpawner.waves = new Wave[0];

        // 5. Configuram EnemyCounter static și managerii globali
        dummyManagerObj = new GameObject("DummyManagers");
        EnemyCounter.instance = dummyManagerObj.AddComponent<EnemyCounter>();
        
        GameObject counterTextObj = new GameObject("CounterTextObj");
        EnemyCounter.instance.enemyText = counterTextObj.AddComponent<TextMeshProUGUI>();
        EnemyCounter.enemiesAlive = 0;
        
        // Asigurăm prezența SoundEffectsManager în caz că GameManager îl apelează la WinLevel
        if (SoundEffectsManager.instance == null)
        {
            SoundEffectsManager.instance = dummyManagerObj.AddComponent<SoundEffectsManager>();
        }

        PlayerStats.rounds = 0;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(spawnerObject);
        if (dummyManagerObj != null)
        {
            Object.DestroyImmediate(dummyManagerObj);
        }
        
        // Curățăm și celelalte obiecte de UI create local în Setup
        GameObject txt1 = GameObject.Find("WaveTextObj");
        if (txt1 != null) Object.DestroyImmediate(txt1);
        
        GameObject txt2 = GameObject.Find("CounterTextObj");
        if (txt2 != null) Object.DestroyImmediate(txt2);

        GameObject gm = GameObject.Find("GameManagerObj");
        if (gm != null) Object.DestroyImmediate(gm);

        GameObject sp = GameObject.Find("SpawnPointObj");
        if (sp != null) Object.DestroyImmediate(sp);
    }

    [Test]
    public void WaveSpawner_Initializes_WithCorrectCountdown()
    {
        // ACT
        waveSpawner.initialCountdown = 12f;
        waveSpawner.Awake();

        // ASSERT
        System.Reflection.FieldInfo countdownField = typeof(WaveSpawner).GetField("countdown", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        float countdownValue = (float)countdownField.GetValue(waveSpawner);

        Assert.AreEqual(12f, countdownValue);
    }

    [Test]
    public void WaveSpawner_TriggersWinLevel_WhenAllWavesDone_AndNoEnemiesAlive()
    {
        // ARRANGE
        EnemyCounter.enemiesAlive = 0;

        // Setăm waveNumber să fie egal cu lungimea vectorului waves (0) folosind reflexia
        System.Reflection.FieldInfo waveNumberField = typeof(WaveSpawner).GetField("waveNumber", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        waveNumberField.SetValue(waveSpawner, 0); 

        // ACT - Forțăm executarea metodei Update din spawner
        System.Reflection.MethodInfo updateMethod = typeof(WaveSpawner).GetMethod("Update", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Deoarece GameManager.WinLevel() este acum "safe" și nu mai aruncă excepții, 
        // putem apela Update-ul direct, fără blocuri try-catch!
        updateMethod.Invoke(waveSpawner, null);

        // ASSERT - Verificăm dacă spawnerul s-a oprit singur (this.enabled = false) conform logicii tale
        Assert.IsFalse(waveSpawner.enabled, "WaveSpawner ar trebui să se dezactiveze automat când nivelul este câștigat!");

        // Verificăm și dacă GameManager-ul a trecut starea jocului pe GameOver/Won
        Assert.IsTrue(GameManager.gameIsOver, "GameManager ar trebui să marcheze jocul ca fiind terminat (gameIsOver = true)!");
    }

    [Test]
    public void WaveSpawner_TutorialPaused_PreventsCountdown()
    {
        // ARRANGE
        waveSpawner.isTutorialLVL = true;
        waveSpawner.tutorialPaused = true;
        waveSpawner.initialCountdown = 10f;
        waveSpawner.Awake(); 

        // ACT
        System.Reflection.MethodInfo updateMethod = typeof(WaveSpawner).GetMethod("Update", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        updateMethod.Invoke(waveSpawner, null);

        // ASSERT
        System.Reflection.FieldInfo countdownField = typeof(WaveSpawner).GetField("countdown", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        float countdownValue = (float)countdownField.GetValue(waveSpawner);

        Assert.AreEqual(10f, countdownValue);
    }
}