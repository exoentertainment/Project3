using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class TurretAttack : MonoBehaviour
{
    #region -- Serialized Fields --

    [FormerlySerializedAs("platformTurretSO")]
    [Header("Scriptable Object")]
    [SerializeField] TurretSO turretSO;

    [Header("Components")] 
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] barrelTransform;

    #endregion

    float lastFireTime;
    GameObject target;

    private void Start()
    {
        lastFireTime = Time.time;
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            SearchForTarget();

            if (target != null)
            {
                RotateTurret();
                RotateBarrel();
                Fire();
            }
        }
    }

    void SearchForTarget()
    {
        if (target == null || Vector3.Distance(transform.position, target.transform.position) > turretSO.attackRange)
        {
            float closestEnemy = Mathf.Infinity;

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, turretSO.attackRange, turretSO.targetLayer);

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
    }

    void Fire()
    {
        if ((Time.time - lastFireTime) >= turretSO.attackSpeed)
        {
            StartCoroutine(FireRoutine());

            lastFireTime = Time.time;
        }
    }

    IEnumerator FireRoutine()
    {
        for (int x = 0; x < barrelTransform.Length; x++)
        {
            Vector3 targetVector = target.transform.position - barrelTransform[x].position;
            targetVector.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(targetVector);
            
            Instantiate(turretSO.dischargePrefab, spawnPoints[x].position, Quaternion.identity);
            
            if(CameraManager.instance.IsObjectInView(transform))
                AudioManager.instance.PlayTurretSound();
            
            Instantiate(turretSO.projectilePrefab, spawnPoints[x].position, targetRotation);
            
            yield return new WaitForSeconds(turretSO.delayPerBarrel);
        }
    }
    
    void RotateTurret()
    {
        Vector3 targetVector = target.transform.position - transform.position;
        targetVector.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(targetVector);
            
        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
    }

    void RotateBarrel()
    {
        foreach (Transform barrels in barrelTransform)
        {
            barrels.LookAt(target.transform);
        }
    }

    bool CheckLineOfSight()
    {
        Ray ray = new Ray(transform.position, target.transform.position - transform.position);
        
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider != null)
            {
                if (1 << hit.collider.gameObject.layer == turretSO.targetLayer)
                {
                    return true;
                }
            }
        }
        else
        {
            return false;
        }
        
        return false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, turretSO.attackRange);
    }
}