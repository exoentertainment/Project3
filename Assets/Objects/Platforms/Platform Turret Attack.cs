using System;
using UnityEngine;

public class PlatformTurretAttack : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")] [SerializeField]
    PlatformTurretSO platformTurretSO;

    [Header("Prefab Object")] [SerializeField]
    GameObject projectilePrefab;

    [Header("Components")] 
    [SerializeField] private Transform spawnPoint;
    
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
        SearchForTarget();
        RotateTowardsTarget();
        Fire();
    }

    void SearchForTarget()
    {
        if(Time.timeScale == 1)
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
                
                if(target != null)
                    Debug.Log("target found");
            }
    }

    void Fire()
    {
        if(Time.timeScale == 1)
            if (target != null)
            {
                if ((Time.time - lastFireTime) >= platformTurretSO.attackSpeed)
                {
                    Vector3 targetVector = target.transform.position - transform.position;
                    targetVector.Normalize();
                    float rotateAmountZ = Vector3.Cross(targetVector, transform.forward).z;
                    float rotateAmountX = Vector3.Cross(targetVector, transform.forward).x;
                    float rotateAmountY = Vector3.Cross(targetVector, transform.forward).y;
            
                    float newAngleZ = transform.rotation.eulerAngles.z + (-rotateAmountZ);
                    float newAngleX = transform.rotation.eulerAngles.x + (-rotateAmountX);
                    float newAngleY = transform.rotation.eulerAngles.y + (-rotateAmountY);
                    
                    Instantiate(projectilePrefab, spawnPoint.position, Quaternion.Euler(newAngleX, newAngleY, newAngleZ));
                    lastFireTime = Time.time;
                }
            }
    }

    void RotateTowardsTarget()
    {
        if(Time.timeScale == 1)
            if (target != null)
            {
                Vector3 targetVector = target.transform.position - transform.position;
                targetVector.Normalize();
                float rotateAmountY = Vector3.Cross(targetVector, transform.forward).y;

                float newAngleY = transform.rotation.eulerAngles.y - rotateAmountY;
                transform.rotation = Quaternion.Euler(0, newAngleY, 0);
            }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, platformTurretSO.attackRange);
    }
}