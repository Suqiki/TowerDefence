using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;

    public int health = 100;

    public int value = 50;
    
    public GameObject deathEffect;

    private Transform target;
    private int wavepointIndex = 0;
   

    void Start()
    {
        if (Waypoints.points == null || Waypoints.points.Length == 0)
        {
            Debug.LogError("Waypoints not set!");
            return;
        }

        target = Waypoints.points[0];
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        
        if (Vector3.Distance(transform.position, target.position) < 0.25f)
        {
            GetNextWaypoint();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerStats.Gold += value;
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(effect, 5f);
        EnemyCounter.instance.MinusEnemy();
    }

    private void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            EnemyCounter.instance.MinusEnemy();
            //EnemyCounter.enemiesAlive--;
            return;
        }
        
        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }
    
    void EndPath()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }
}
