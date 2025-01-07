using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class StationaryMissileLauncher : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] TurretSO platformTurretSO;

    [Header("Components")] 
    [SerializeField] private Transform[] spawnPoints;

    #endregion

    float lastFireTime;
    GameObject target = null;

    private void Start()
    {
        lastFireTime = Time.time;
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            //SearchForTarget();
            Fire();
        }
    }

    void SearchForTarget()
    {
        if (target == null)
        {
            float closestEnemy = Mathf.Infinity;

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, platformTurretSO.attackRange, platformTurretSO.targetLayer);

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
        else
        {
            SearchForTarget();
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
            
            Instantiate(platformTurretSO.projectilePrefab, spawnPoint.position, transform.rotation);
            
            yield return new WaitForSeconds(platformTurretSO.delayPerBarrel);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, platformTurretSO.attackRange);
    }
}
