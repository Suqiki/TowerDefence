using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;
    
    private int waveNumber = 0;

    public TextMeshProUGUI waveText;
    
    private void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        countdown -= Time.deltaTime;
        
        waveText.text = $"Next wave in {Mathf.Floor(countdown)} sec.";
    }

    IEnumerator SpawnWave()
    {
        waveNumber++;
        
        Debug.Log("Spawning Wave");

        for (int i = 0; i < waveNumber; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
        
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyCounter.instance.PlusEnemy();
        //EnemyCounter.enemiesAlive++;
    }
}
