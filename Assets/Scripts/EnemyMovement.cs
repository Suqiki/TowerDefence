using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int wavepointIndex = 0;

    private Enemy _enemy;
    
    void Start()
    {
        if (Waypoints.points == null || Waypoints.points.Length == 0)
        {
            Debug.LogError("Waypoints not set!");
            return;
        }

        _enemy = GetComponent<Enemy>();
        
        target = Waypoints.points[0];
    }
    
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * _enemy.speed * Time.deltaTime, Space.World);
        
        if (Vector3.Distance(transform.position, target.position) < 0.25f)
        {
            GetNextWaypoint();
        }
    }
    
    private void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }
        
        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }
    
    void EndPath()
    {
        if (_enemy.isDead)
            return;

        _enemy.isDead = true;

        PlayerStats.Lives = Mathf.Max(0, PlayerStats.Lives - 1);
        
        EnemyCounter.instance.MinusEnemy();

        Destroy(gameObject);
    }
}
