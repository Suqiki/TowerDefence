using UnityEngine;

public class FireBalll : MonoBehaviour
{
    private Transform target;

    public float speed = 50f;
    public GameObject HitEffect;

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
        transform.LookAt(target);
        
    }

    private void HitTarget()
    {
        //Debug.Log("Hit Target");
        GameObject effectIns = (GameObject)Instantiate(HitEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);
        Destroy(target.gameObject);
        EnemyCounter.instance.MinusEnemy();
        Destroy(gameObject);
    }
}
