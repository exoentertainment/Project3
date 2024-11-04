using System;
using System.Collections;
using UnityEngine;

public class VLSMissile : MonoBehaviour
{
    #region -- Serialized Fields

    [Header("Scriptable Object")]
    [SerializeField] VLSMissileSO missileSO;

    #endregion

    private bool isCoasting = true;
    bool hasCollided = false;
    private GameObject target;

    private void Start()
    {
        FindTarget();
        StartCoroutine(CoastingCountdownRoutine());
    }

    private void Update()
    {
        if(Time.timeScale == 1)
            Move();
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
        if (isCoasting)
        {
            transform.position += transform.forward * (missileSO.coastSpeed * Time.deltaTime);
        }
        else
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
    }

    IEnumerator CoastingCountdownRoutine()
    {
        yield return new WaitForSeconds(missileSO.coastDuration);

        isCoasting = false;
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    
    void SpawnDischargeEffect()
    {
        Instantiate(missileSO.dischargeEffectPrefab, transform.position, transform.rotation);    
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (1 << other.gameObject.layer == missileSO.targetLayer && !hasCollided)
        {
            hasCollided = true;
            
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hit))
            {
                hit.TakeDamage(missileSO.damage);
            }
        }
        
        if(CameraManager.instance.IsObjectInView(transform))
            AudioManager.instance.PlayLargeExplosion();
        
        Instantiate(missileSO.explodeEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, missileSO.attackRange);
        }
}
