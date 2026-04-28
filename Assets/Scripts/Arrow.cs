using UnityEngine;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.Serialization;

public class Arrow : MonoBehaviour
{
    private Transform target;
    public float explosionRadius = 0f;

    public int damage = 50;

    public float speed = 50f;
    [FormerlySerializedAs("effect")] public GameObject HitEffect;

    public void Seek(Transform _target)
    {
        target = _target;
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        Vector3 dir = target.position - transform.position;
        float distanceFrame = speed * Time.deltaTime;

        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = lookRotation;
        
        if (dir.magnitude <= distanceFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceFrame, Space.World);
        
    }

    private void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(HitEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);

        // ținta principala ia damage complet
        Damage(target, damage);

        if (explosionRadius > 0f)
        {
            Explode();
        }

        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Enemy") && hit.transform != target)
            {
                int splashDamage = damage / 2; // 50% damage
                Damage(hit.transform, splashDamage);
            }
        }
    }

    void Damage(Transform enemy, int amount)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.TakeDamage(amount);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
