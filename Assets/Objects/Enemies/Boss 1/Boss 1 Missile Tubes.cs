using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class Boss1MissileTubes : MonoBehaviour
{
        #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] TurretSO platformTurretSO;

    [Header("Prefab Object")] [SerializeField]
    GameObject projectilePrefab;

    [Header("Components")] 
    [SerializeField] private Transform[] spawnPoints;
    
    [Header("Variables")] 
    [SerializeField] LayerMask targetLayer;

    #endregion

    float lastFireTime;
    private bool isVisible;
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
            Fire();
        }
    }

    void SearchForTarget()
    {
        if (target == null)
        {
            float closestEnemy = Mathf.Infinity;

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, platformTurretSO.attackRange, targetLayer);

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
        if (target != null)
        {
            if ((Time.time - lastFireTime) >= platformTurretSO.attackSpeed)
            {
                StartCoroutine(FireRoutine());
                
                lastFireTime = Time.time;
            }
        }
    }

    IEnumerator FireRoutine()
    {
        if(CameraManager.instance.IsObjectInView(transform))
            AudioManager.instance.PlayLightMissileTurretSound();
        
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (target == null)
                break;
            
            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.Euler(-90, 0, 0));
            
            
            yield return new WaitForSeconds(platformTurretSO.delayPerBarrel);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, platformTurretSO.attackRange);
    }
}
