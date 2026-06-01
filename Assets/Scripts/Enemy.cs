using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    [HideInInspector]
    public float speed = 10f;
    public float slowRecoverSpeed = 5f;
    private bool isSlowed = false;
    public bool doubleBaseDmg = false;
    
    public float startHealth = 100;
    private float health;
    private bool isHealthInitialized = false;

    public int value = 50;
    
    public GameObject deathEffect;
    
    [Header("Health")]
    public Image healthBar;
    
    [Header("DoT Effect")]
    public GameObject dotEffectPrefab;
    private GameObject activeDotEffect;
    
    [HideInInspector]
    public float DoT = 0f;
    [HideInInspector]
    public float dotDuration = 0f;

    [HideInInspector]
    public bool isDead = false;
    
    [Header("Sound Effects")]
    public AudioClip deathSound;
    public float deathSoundPower = 1;
    
    void Start()
    {
        Initializehealth();
        speed=startSpeed;
    }

    public void Initializehealth()
    {
        health = startHealth;
        isHealthInitialized = true;
    }
    
    void Update()
    {
        if (isDead)
            return;
        
        if (dotDuration > 0f)
        {
            TakeDamage(DoT * Time.deltaTime);

            dotDuration -= Time.deltaTime;
        }
        else
        {
            DoT = 0f;

            if (activeDotEffect != null)
            {
                Destroy(activeDotEffect);

                activeDotEffect = null;
            }
        }
        
        if (!isSlowed)
        {
            speed = Mathf.MoveTowards(speed, startSpeed, slowRecoverSpeed * Time.deltaTime);
        }
        
        isSlowed = false;
    }

    public void TakeDamage(float damage)
    {
        if (!isHealthInitialized)
        {
            Initializehealth();
        }
        
        health -= damage;
        
        healthBar.fillAmount = health / startHealth;
        
        if (health <= 0 && !isDead)
        {
            if (SoundEffectsManager.instance != null && deathSound != null)
            {
                SoundEffectsManager.instance.PlaySoundEffect(deathSound, transform, deathSoundPower);
            }
            Die();
        }
    }
    
    public void ApplyDoT(float damagePerSecond, float duration)
    {
        DoT = damagePerSecond;
        dotDuration = duration;

        // dacă nu există deja efect activ
        if (activeDotEffect == null)
        {
            activeDotEffect = Instantiate(
                dotEffectPrefab,
                transform.position,
                Quaternion.identity,
                transform
            );
        }
    }

    public void Slow(float amount)
    {
        speed = startSpeed * (1f - amount);
        isSlowed = true;
    }

    void Die()
    {
        if(isDead)
            return;
        
        isDead = true;
        PlayerStats.Gold += value;

        if (deathEffect != null)
        {
            GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            if (Application.isPlaying) Destroy(effect, 5f);
        }

        // Verificări de siguranță pentru Singletons în timpul testării
        EnemyCounter.instance?.MinusEnemy();
        
        if (PlayerProgressManager.instance != null)
        {
            PlayerProgressManager.instance.enemiesKilled++;
            PlayerProgressManager.instance.goldEarned += value;
        }

        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.EnemyKilled(gameObject.name);
        }
        
        if (activeDotEffect != null)
        {
            Destroy(activeDotEffect);
        }

        // În EditMode nu poți folosi Destroy normal pe un GameObject, verificăm dacă jocul rulează efectiv
        if (Application.isPlaying)
        {
            Destroy(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
    
}
