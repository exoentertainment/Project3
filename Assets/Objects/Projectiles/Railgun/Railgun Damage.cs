using UnityEngine;

public class RailgunDamage : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] IProjectileScriptableObject projectileSO;

    private GameObject hitObject;
    
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            //CheckNearbyArea();
        }
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
    
    private void OnCollisionEnter(Collision other)
    {
        Instantiate(projectileSO.explodeEffectPrefab, transform.position, Quaternion.identity);
        
        if (other.gameObject.layer != LayerMask.NameToLayer("Celestial Body"))
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hit))
            {
                hit.TakeDamage(projectileSO.damage);
                Instantiate(projectileSO.explodeEffectPrefab, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, projectileSO.blastRadius);
    }
}
