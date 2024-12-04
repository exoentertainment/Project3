using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LightMissileMove : MonoBehaviour
{
    #region -- Serialized Fields --

    [FormerlySerializedAs("projectileSO")]
    [Header("Scriptable Object")] 
    [SerializeField] private MissileScriptableObject missileSO;

    [SerializeField] private float rotationTime;
    
    #endregion

    private GameObject target;
    float lastRotationTime;
    int swerveDirection = 1;
    private float moveSpeed;
    
    private void Start()
    {
        lastRotationTime = Time.time;
        moveSpeed = Random.Range(missileSO.minMoveSpeed, missileSO.maxMoveSpeed);
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
            target = potentialTargets[UnityEngine.Random.Range(0, potentialTargets.Length)].gameObject;
        }
    }
    
    void Move()
    {
        transform.position += transform.forward * (moveSpeed * Time.deltaTime);

        if (target != null)
        {
            if (CheckLineOfSight(target))
                FindTarget();
            
            transform.LookAt(target.transform, transform.up);
        }
        else
        {
            FindTarget();
        }
        
        float noiseyness = 1; //tweak this to adjust how random the motion is
        
        transform.Translate(transform.up * swerveDirection * 1 * Time.deltaTime);
        transform.Translate(transform.right * swerveDirection * 1 * Time.deltaTime);

        if ((Time.time - lastRotationTime) > rotationTime)
        {
            lastRotationTime = Time.time;
            swerveDirection *= -1;
        }
    }
    

    void SpawnDischargeEffect()
    {
        if(missileSO.dischargeEffectPrefab != null)
            Instantiate(missileSO.dischargeEffectPrefab, transform.position, transform.rotation);    
    }
    
    bool CheckLineOfSight(GameObject target)
    {
        if (target != null)
        {
            Ray ray = new Ray(transform.position, target.transform.position - transform.position);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Celestial Body"))
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        return false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hit))
        {
            hit.TakeDamage(missileSO.damage);
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
