using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

public class BossRailgunTurret : MonoBehaviour
{
    #region -- Serialized Fields --

    [FormerlySerializedAs("platformTurretSO")]
    [Header("Scriptable Object")]
    [SerializeField] TurretSO turretSO;

    [Header("Components")] 
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] barrelTransform;

    [Header("Variables")]
    [SerializeField] private float fireRateMultiplier;
    [SerializeField] MMFeedbacks firingFeedback;
    
    #endregion

    float lastFireTime;
    private bool isFastFiring;
    GameObject target;

    private void Start()
    {
        lastFireTime = Time.time;
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            if (target == null)
            {
                SearchForTarget();
            }

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

                    if (distanceToEnemy < closestEnemy && !CheckLineOfSight(potentialTargets[x].gameObject))
                    {
                        closestEnemy = distanceToEnemy;
                        target = potentialTargets[x].gameObject;
                    }
                }
            }
        }
    }

    public void SearchForPlanet()
    {
        float closestEnemy = Mathf.Infinity;

        Collider[] potentialTargets =
            Physics.OverlapSphere(transform.position, Mathf.Infinity, LayerMask.GetMask("Celestial Body"));

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
    
    void Fire()
    {
        if (!isFastFiring)
        {
            if((Time.time - lastFireTime) >= turretSO.attackSpeed)
            {
                if (!CheckLineOfSight(target))
                {
                    StartCoroutine(FireRoutine());

                    lastFireTime = Time.time;
                }
                else
                {
                    target = null;
                }
            }
        }
        else
        {
            if ((Time.time - lastFireTime) >= (turretSO.attackSpeed * fireRateMultiplier))
            {
                if (!CheckLineOfSight(target))
                {
                    StartCoroutine(FireRoutine());

                    lastFireTime = Time.time;
                }
                else
                {
                    target = null;
                }
            }
        }
    }

    IEnumerator FireRoutine()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (target != null)
            {
                Vector3 targetVector = target.transform.position - spawnPoint.position;
                targetVector.Normalize();

                Quaternion targetRotation = Quaternion.LookRotation(targetVector);

                Instantiate(turretSO.dischargePrefab, spawnPoint.position, barrelTransform[0].rotation);

                if (CameraManager.instance.IsObjectInView(transform))
                {
                    AudioManager.instance.PlayTurretSound();
                    firingFeedback?.PlayFeedbacks();
                }

                Instantiate(turretSO.projectilePrefab, spawnPoint.position, targetRotation);
            }

            yield return new WaitForSeconds(turretSO.delayPerBarrel);
        }
    }

    public void IncreaseFireRate()
    {
        isFastFiring = true;
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
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, turretSO.attackRange);
    }
}
