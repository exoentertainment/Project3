using UnityEngine;
using UnityEngine.Serialization;

public class LightMissileMove : MonoBehaviour
{
    #region -- Serialized Fields --

    [FormerlySerializedAs("projectileSO")]
    [Header("Scriptable Object")] 
    [SerializeField] private MissileScriptableObject missileSO;

    #endregion

    private GameObject target;
    
    private void Start()
    {
        FindTarget();
        SpawnDischargeEffect();
        Destroy(gameObject, missileSO.lifeTime);
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            Move();
        }
    }

    void FindTarget()
    {
        float closestEnemy = Mathf.Infinity;

        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, missileSO.attackRange, missileSO.targetLayer);

        if (potentialTargets.Length > 0)
        {
            for (int x = 0; x < potentialTargets.Length; x++)
            {
                float distanceToEnemy =
                    Vector3.Distance(potentialTargets[x].transform.position, transform.position);

                if (distanceToEnemy < closestEnemy)
                {
                    closestEnemy = distanceToEnemy;
                    target = potentialTargets[x].gameObject;
                }
            }
        }
    }
    
    void Move()
    {
        if (target != null)
        {
            transform.LookAt(target.transform, transform.up);
        }
        else
        {
            FindTarget();
        }
        
        transform.position += transform.forward * (missileSO.moveSpeed * Time.deltaTime);
    }

    void SpawnDischargeEffect()
    {
        Instantiate(missileSO.dischargeEffectPrefab, transform.position, transform.rotation);    
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (1 << other.gameObject.layer == missileSO.targetLayer)
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hit))
            {
                hit.TakeDamage(missileSO.damage);
            }
        }

        if(CameraManager.instance.IsObjectInView(transform))
            AudioManager.instance.PlaySmallExplosion();
            
        Instantiate(missileSO.explodeEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, missileSO.attackRange);
    }
}
