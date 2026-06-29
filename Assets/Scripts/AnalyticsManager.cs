using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    private bool isInitialized = false;

    async void Awake()
    {
        Instance = this;
        
        try
        {
            await UnityServices.InitializeAsync();

            AnalyticsService.Instance.StartDataCollection();

            isInitialized = true;

            Debug.Log("Unity Analytics initialized!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Analytics init failed: " + e.Message);
        }
    }

    // =========================
    // ENEMY KILLED
    // =========================
    public void EnemyKilled(string enemyType)
    {
        if (!isInitialized) return;

        CustomEvent myEvent = new CustomEvent("enemy_killed")
        {
            { "enemy_type", enemyType }
        };

        AnalyticsService.Instance.RecordEvent(myEvent);
    }

    // =========================
    // LEVEL COMPLETED
    // =========================
    public void LevelCompleted(string levelName, int stars)
    {
        if (!isInitialized) return;

        CustomEvent myEvent = new CustomEvent("level_completed")
        {
            { "level_name", levelName },
            { "stars", stars }
        };

        AnalyticsService.Instance.RecordEvent(myEvent);
    }

    // =========================
    // LEVEL FAILED
    // =========================
    public void LevelFailed(string levelName)
    {
        if (!isInitialized) return;

        CustomEvent myEvent = new CustomEvent("level_failed")
        {
            { "level_name", levelName }
        };

        AnalyticsService.Instance.RecordEvent(myEvent);
    }

    // =========================
    // TURRET BOUGHT
    // =========================
    public void TurretBought(string turretName)
    {
        if (!isInitialized) return;

        CustomEvent myEvent = new CustomEvent("turret_bought")
        {
            { "turret_name", turretName }
        };

        AnalyticsService.Instance.RecordEvent(myEvent);
    }

    // =========================
    // TURRET UPGRADED
    // =========================
    public void TurretUpgraded(string turretName, int level)
    {
        if (!isInitialized) return;

        CustomEvent myEvent = new CustomEvent("turret_upgraded")
        {
            { "turret_name", turretName },
            { "upgrade_level", level }
        };

        AnalyticsService.Instance.RecordEvent(myEvent);
    }
}