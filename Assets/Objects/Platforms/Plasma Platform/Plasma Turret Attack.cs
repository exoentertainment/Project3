using System;
using UnityEngine;
using System.Collections;
using MoreMountains.Feedbacks;

public class PlasmaTurretAttack : MonoBehaviour
{
    #region -- Serialized Fields --
    
    [Header("Scriptable Object")]
    [SerializeField] TurretSO turretSO;

    [Header("Components")] 
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] barrelTransform;

    [SerializeField] MMFeedbacks firingFeedback;
    
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
            if(target == null)
                SearchForTarget();
            else
            {
                RotateTurret();
                RotateBarrel();
                Fire();
            }
        }
    }

    void SearchForTarget()
    {
        if (target == null)
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

    private void FixedUpdate()
    {
        SearchForTarget();
    }

    void Fire()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > turretSO.attackRange)
            {
                target = null;
                return;
            }
            
            if ((Time.time - lastFireTime) >= turretSO.attackSpeed)
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
        // if(target != null)
        //     for (int x = 0; x < spawnPoints.Length; x++)
        //     {
        //         Vector3 targetVector = target.transform.position - spawnPoints[x].position;
        //         targetVector.Normalize();
        //         float rotateAmountZ = Vector3.Cross(targetVector, barrelTransform[0].forward).z;
        //         float rotateAmountX = Vector3.Cross(targetVector, barrelTransform[0].forward).x;
        //         float rotateAmountY = Vector3.Cross(targetVector, barrelTransform[0].forward).y;
        //         
        //         float newAngleZ = barrelTransform[0].rotation.eulerAngles.z + (-rotateAmountZ);
        //         float newAngleX = barrelTransform[0].rotation.eulerAngles.x + (-rotateAmountX);
        //         float newAngleY = barrelTransform[0].rotation.eulerAngles.y + (-rotateAmountY);
        //         
        //         if(CameraManager.instance.IsObjectInView(transform))
        //             AudioManager.instance.PlayPlasmaTurretSound();
        //         
        //         GameObject projectile = Instantiate(turretSO.projectilePrefab, spawnPoints[x].position, Quaternion.identity);
        //         projectile.transform.rotation = Quaternion.Euler(newAngleX, newAngleY, newAngleZ);
        //         
        //         yield return new WaitForSeconds(turretSO.delayPerBarrel);
        //     }

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (target != null)
            {
                Vector3 targetVector = target.transform.position - spawnPoint.position;
                targetVector.Normalize();

                Quaternion targetRotation = Quaternion.LookRotation(targetVector);

                if (turretSO.dischargePrefab != null)
                    Instantiate(turretSO.dischargePrefab, spawnPoint.position, Quaternion.identity);

                if (CameraManager.instance.IsObjectInView(transform))
                {
                    AudioManager.instance.PlayPlasmaTurretSound();
                    firingFeedback?.PlayFeedbacks();
                }

                Instantiate(turretSO.projectilePrefab, spawnPoint.position, targetRotation);
            }
            else
            {
                break;
            }
            
            yield return new WaitForSeconds(turretSO.delayPerBarrel);
        }
    }

    void RotateTurret()
    {
        if (target != null)
        {
            Vector3 targetVector = target.transform.position - transform.position;
            targetVector.Normalize();
            float rotateAmountY = Vector3.Cross(targetVector, transform.forward).y;
            
            float newAngleY = transform.rotation.eulerAngles.y + (-rotateAmountY);
            
            transform.rotation = Quaternion.Euler(0, newAngleY, 0);
        }
    }

    void RotateBarrel()
    {
        if (target != null)
        {
            foreach (Transform barrels in barrelTransform)
            {
                barrels.LookAt(target.transform);
            }
            
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
