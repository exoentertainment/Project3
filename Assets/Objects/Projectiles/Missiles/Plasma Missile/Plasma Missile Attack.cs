using UnityEngine;

public class PlasmaMissileAttack : MonoBehaviour
{
    [Header("Scriptable Object")] 
    [SerializeField] private MissileScriptableObject missileSO;
    
    [Header("Variables")]
    [SerializeField] float blastRadius;
    
    private void OnCollisionEnter(Collision other)
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, blastRadius, missileSO.targetLayer);
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
        
        Instantiate(missileSO.explodeEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    void DealDamage(IDamageable target)
    {
        target.TakeDamage(missileSO.damage);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
