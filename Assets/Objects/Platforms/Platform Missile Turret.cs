using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class PlatformMissileTurret : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] TurretSO platformTurretSO;

    [Header("Components")] 
    [SerializeField] private Transform[] spawnPoints;
    [FormerlySerializedAs("tubeTransform")] [SerializeField] private Transform missileTubeTransform;
    
    [Header("Variables")] 
    [SerializeField] LayerMask targetLayer;

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
            RotateTurret();
            RotateBarrel();
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

                    if(!CheckLineOfSight())
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
            
            Instantiate(platformTurretSO.projectilePrefab, spawnPoint.position, transform.rotation);
            
            yield return new WaitForSeconds(platformTurretSO.delayPerBarrel);
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
            missileTubeTransform.LookAt(target.transform);
            
        }
    }
    
    bool CheckLineOfSight()
    {
        Ray ray = new Ray(transform.position, target.transform.position - transform.position);
        
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Celestial Body"))
            {
                return true;
            }
            else
            {
                return false;
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
        Gizmos.DrawWireSphere(transform.position, platformTurretSO.attackRange);
    }
}
