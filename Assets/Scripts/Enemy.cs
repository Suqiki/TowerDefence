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
        health = startHealth;
        speed=startSpeed;
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
        health -= damage;
        
        healthBar.fillAmount = health / startHealth;
        
        if (health <= 0 && !isDead)
        {
            SoundEffectsManager.instance.PlaySoundEffect(deathSound, transform, deathSoundPower);
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
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        EnemyCounter.instance.MinusEnemy();
        
        PlayerProgressManager.instance.enemiesKilled++;
        PlayerProgressManager.instance.goldEarned += value;
        
        if (activeDotEffect != null)
        {
            Destroy(activeDotEffect);
        }
        Destroy(gameObject);
        Destroy(effect, 5f);
    }

    
}
