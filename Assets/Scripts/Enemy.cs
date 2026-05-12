using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    [HideInInspector]
    public float speed = 10f;
    public float slowRecoverSpeed = 5f;
    private bool isSlowed = false;
    
    public float health = 100;

    public int value = 50;
    
    public GameObject deathEffect;
    
    [HideInInspector]
    public float DoT = 0f;
    [HideInInspector]
    public float dotDuration = 0f;

    [HideInInspector]
    public bool isDead = false;
    
    void Start()
    {
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
        
        if (!isSlowed)
        {
            speed = Mathf.MoveTowards(speed, startSpeed, slowRecoverSpeed * Time.deltaTime);
        }
        
        isSlowed = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    
    public void ApplyDoT(float damagePerSecond, float duration)
    {
        DoT = damagePerSecond;
        dotDuration = duration;
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
        Destroy(gameObject);
        Destroy(effect, 5f);
    }

    
}
