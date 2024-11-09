using System;
using UnityEngine;

public class PlasmaProjectileAttack : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] IProjectileScriptableObject projectileSO;
    
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            CheckNearbyArea();
        }
    }
    
    void CheckNearbyArea()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, projectileSO.blastRadius, projectileSO.targetLayer);
        float closestEnemy = Mathf.Infinity;

        if (potentialTargets.Length > 0)
        {
            foreach (Collider target in potentialTargets)
            {
                if (target.gameObject.TryGetComponent<IDamageable>(out IDamageable hit))
                {
                    DealDamage(hit);
                }
            }
        }
    }

    void DealDamage(IDamageable target)
    {
        target.TakeDamage(projectileSO.damage);
        Instantiate(projectileSO.explodeEffectPrefab, transform.position, Quaternion.identity);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, projectileSO.blastRadius);
    }

    private void OnCollisionEnter(Collision other)
    {
        Instantiate(projectileSO.explodeEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
