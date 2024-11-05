using System;
using System.Collections;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] IProjectileScriptableObject projectileSO;

    private void Start()
    {
        Destroy(gameObject, projectileSO.lifeTime);
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            Move();
            CheckNearbyArea();
        }
    }

    void Move()
    {
        transform.position += transform.forward * (projectileSO.moveSpeed * Time.deltaTime);
    }

    void CheckNearbyArea()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, projectileSO.blastRadius, projectileSO.targetLayer);
        float closestEnemy = Mathf.Infinity;
        GameObject target = null;

        if (potentialTargets.Length > 0)
        {
            /*foreach (Collider target in potentialTargets)
            {
                if (target.gameObject.TryGetComponent<IDamageable>(out IDamageable hit))
                {
                    hit.TakeDamage(projectileSO.damage);
                    Instantiate(projectileSO.explodeEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }*/
            
            for (int x = 0; x < potentialTargets.Length; x++)
            {
                float distanceToEnemy = Vector3.Distance(potentialTargets[x].transform.position, transform.position);

                if (distanceToEnemy < closestEnemy)
                {
                    closestEnemy = distanceToEnemy;
                    target = potentialTargets[x].gameObject;
                }
            }
            
            if (target.TryGetComponent<IDamageable>(out IDamageable hit))
            {
                hit.TakeDamage(projectileSO.damage);
                Instantiate(projectileSO.explodeEffectPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
    
    // private void OnCollisionEnter(Collision other)
    // {
    //     if (1 << other.gameObject.layer == projectileSO.targetLayer)
    //     {
    //         if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hit))
    //         {
    //             hit.TakeDamage(projectileSO.damage);
    //             
    //         }
    //     }
    //     
    //     Destroy(gameObject);
    // }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, projectileSO.blastRadius);
    }
}
