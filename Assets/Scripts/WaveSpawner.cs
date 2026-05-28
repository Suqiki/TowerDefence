using System.Collections;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    public float initialCountdown = 12;
    private float countdown = 12f;
    public bool tutorialPaused = false;
    public bool isTutorialLVL = false;
    
    private int waveNumber = 0;
    private bool isSpawning = false;

    public TextMeshProUGUI waveText;
    
    public GameManager gameManager;

    public void Awake()
    {
        countdown = initialCountdown;
    }

    private void Update()
    {
        if (isTutorialLVL)
        {
            if (tutorialPaused) 
                return;
        }
        
        if (EnemyCounter.enemiesAlive > 0)
        {
            return;
        }
        
        if (waveNumber >= waves.Length && EnemyCounter.enemiesAlive <= 0)
        {
            gameManager.WinLevel();
            this.enabled = false;
            return;
        }
        
        if (waveNumber >= waves.Length)
        {
            return;
        }
        
        
        
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }
        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        waveText.text = $"Next wave in {Mathf.Floor(countdown)} sec.";
    }
    
    public void StartFirstWaveFromTutorial()
    {
        StartCoroutine(SpawnTutorialWave());
    }
    
    IEnumerator SpawnTutorialWave()
    {
        tutorialPaused = false;

        yield return StartCoroutine(SpawnWave());

        // Așteaptă până mor toți inamicii
        yield return new WaitUntil(() => EnemyCounter.enemiesAlive <= 0);

        tutorialPaused = true;
    }

    IEnumerator SpawnWave()
    {
        
        PlayerStats.rounds++;
        
        //Debug.Log("Spawning Wave");
        
        Wave wave = waves[waveNumber];

        foreach (EnemyGroup group in wave.enemies )
        {
            for (int i = 0; i < group.count; i++)
            {
                SpawnEnemy(group.enemy);
                yield return new WaitForSeconds(1f/group.rate);
            }
        }
        
        waveNumber++;
    }

    private void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        EnemyCounter.instance.PlusEnemy();
        //EnemyCounter.enemiesAlive++;
    }
}
