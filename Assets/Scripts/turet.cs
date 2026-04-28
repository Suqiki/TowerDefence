using System;
using UnityEngine;

public class turet : MonoBehaviour
{
    public Transform target;
    
    [Header("General")]
    public float range = 15f;
    
    [Header("Use Bullets (default)")]
    public GameObject Arrow;
    public float fireRate = 1f;
    public float fireCountdown = 0f;

    [Header("Use Laser")]
    public int damageOverTime = 30;
    public bool useLaser = false;
    public LineRenderer LineRenderer;
    public ParticleSystem impactEffect;
    
    [Header("Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform PartToRotate;
    public float rotateSpeed = 5f;
    public Transform FirePoint;
    
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("UpdateTarget", 1f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (useLaser)
            {
                if (LineRenderer.enabled)
                {
                    LineRenderer.enabled = false;
                    impactEffect.Stop();
                }
            }
            return;
        }
        
        LockOnTarget();

        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(PartToRotate.rotation, lookRotation, Time.deltaTime * rotateSpeed).eulerAngles;
        PartToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Laser()
    {
        if (!LineRenderer.enabled)
        {
            LineRenderer.enabled = true;
            impactEffect.Play();
        }

        LineRenderer.SetPosition(0, FirePoint.position);
        LineRenderer.SetPosition(1, target.position);

        Vector3 dir = FirePoint.position - target.position;
        
        impactEffect.transform.position = target.position +  dir.normalized * 0.5f;
        
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
        
        // distanța până la target
        float distance = Vector3.Distance(transform.position, target.position);

        // 0 = departe, 1 = aproape
        float closeness = 1f - (distance / range);

        // limitează între 0 și 1
        closeness = Mathf.Clamp01(closeness);

        // grosimea minimă și maximă
        float minWidth = 0.1f;
        float maxWidth = 0.5f;

        float width = Mathf.Lerp(minWidth, maxWidth, closeness);

        LineRenderer.startWidth = width;
        LineRenderer.endWidth = width;
    }

    private void Shoot()
    {
        Debug.Log("shoot");
        GameObject ArrowGo = (GameObject)Instantiate(Arrow, FirePoint.position, FirePoint.rotation);
        Arrow arrow = ArrowGo.GetComponent<Arrow>();
        
        if(arrow != null)
            arrow.Seek(target);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearstEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (shortestDistance > distanceToEnemy)
            {
                shortestDistance = distanceToEnemy;
                nearstEnemy = enemy;
            }
        }

        if (nearstEnemy != null && shortestDistance <= range)
        {
            target = nearstEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
