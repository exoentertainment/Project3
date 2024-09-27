using System;
using UnityEngine;

public class PlatformTurretAttack : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")] [SerializeField]
    PlatformTurretSO platformTurretSO;

    [Header("Prefab Object")] [SerializeField]
    GameObject projectilePrefab;

    [Header("Variables")] [SerializeField] LayerMask targetLayer;

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
                if (Time.time - lastFireTime >= platformTurretSO.attackSpeed)
                {
                    
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