using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using MoreMountains.Feedbacks;

public class RotaryTurretAttack : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] private float burstAmount;
    
    [FormerlySerializedAs("platformTurretSO")]
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

    void Fire()
    {
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

    IEnumerator FireRoutine()
    {
        for (int i = 0; i < burstAmount; i++)
        {
            if (CameraManager.instance.IsObjectInView(transform))
            {
                AudioManager.instance.PlayTurretSound();
                firingFeedback?.PlayFeedbacks();
            }
            
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (target != null)
                {
                    Vector3 targetVector = target.transform.position - spawnPoint.position;
                    targetVector.Normalize();

                    Quaternion targetRotation = Quaternion.LookRotation(targetVector);

                    if(turretSO.dischargePrefab != null)
                        Instantiate(turretSO.dischargePrefab, spawnPoint.position, Quaternion.identity);

                    Instantiate(turretSO.projectilePrefab, spawnPoint.position, targetRotation);
                }

                yield return new WaitForSeconds(turretSO.delayPerBarrel);
            }
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