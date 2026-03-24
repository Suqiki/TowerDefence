using UnityEngine;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.Serialization;

public class Arrow : MonoBehaviour
{
    private Transform target;
    public float explosionRadius = 0f;


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
        //Debug.Log("Hit Target");
        GameObject effectIns = (GameObject)Instantiate(HitEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }
        
       
        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.tag == "Enemy")
            {
                Damage(hit.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Destroy(enemy.gameObject);
        EnemyCounter.instance.MinusEnemy();
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
