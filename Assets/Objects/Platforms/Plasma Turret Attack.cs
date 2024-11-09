using UnityEngine;
using System.Collections;

public class PlasmaTurretAttack : MonoBehaviour
{
    #region -- Serialized Fields --
    
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
            RotateTurret();
            RotateBarrel();
            Fire();
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
        if (target != null)
        {
            if ((Time.time - lastFireTime) >= turretSO.attackSpeed)
            {
                StartCoroutine(FireRoutine());

                lastFireTime = Time.time;
            }
        }
    }

    IEnumerator FireRoutine()
    {
        for (int x = 0; x < spawnPoints.Length; x++)
        {
            Vector3 targetVector = target.transform.position - spawnPoints[x].position;
            targetVector.Normalize();
            float rotateAmountZ = Vector3.Cross(targetVector, barrelTransform[0].forward).z;
            float rotateAmountX = Vector3.Cross(targetVector, barrelTransform[0].forward).x;
            float rotateAmountY = Vector3.Cross(targetVector, barrelTransform[0].forward).y;
            
            float newAngleZ = barrelTransform[0].rotation.eulerAngles.z + (-rotateAmountZ);
            float newAngleX = barrelTransform[0].rotation.eulerAngles.x + (-rotateAmountX);
            float newAngleY = barrelTransform[0].rotation.eulerAngles.y + (-rotateAmountY);
            
            if(CameraManager.instance.IsObjectInView(transform))
                AudioManager.instance.PlayPlasmaTurretSound();
            
            GameObject projectile = Instantiate(turretSO.projectilePrefab, spawnPoints[x].position, Quaternion.identity);
            projectile.transform.rotation = Quaternion.Euler(newAngleX, newAngleY, newAngleZ);
            
            yield return new WaitForSeconds(turretSO.delayPerBarrel);
        }
    }
    
    void RotateTurret()
    {
        if (target != null)
        {
            Vector3 targetVector = target.transform.position - transform.position;
            targetVector.Normalize();
            // float rotateAmountZ = Vector3.Cross(targetVector, transform.forward).z;
            // float rotateAmountX = Vector3.Cross(targetVector, transform.forward).x;
            float rotateAmountY = Vector3.Cross(targetVector, transform.forward).y;
            
            // float newAngleZ = transform.rotation.eulerAngles.z + (-rotateAmountZ);
            // float newAngleX = transform.rotation.eulerAngles.x + (-rotateAmountX);
            float newAngleY = transform.rotation.eulerAngles.y + (-rotateAmountY);
            
            transform.rotation = Quaternion.Euler(0, newAngleY, 0);
            
            //transform.LookAt(target.transform);
        }
    }

    void RotateBarrel()
    {
        if (target != null)
        {
            // Vector3 targetVector = target.transform.position - barrelTransform.position;
            // targetVector.Normalize();
            // // float rotateAmountZ = Vector3.Cross(targetVector, transform.forward).z;
            // float rotateAmountX = Vector3.Cross(targetVector, barrelTransform.forward).x;
            // // float rotateAmountY = Vector3.Cross(targetVector, barrelTransform.forward).y;
            //
            // // float newAngleZ = transform.rotation.eulerAngles.z + (-rotateAmountZ);
            // float newAngleX = barrelTransform.rotation.eulerAngles.x + (-rotateAmountX);
            // // float newAngleY = barrelTransform.rotation.eulerAngles.y + (-rotateAmountY);
            //
            // barrelTransform.rotation = Quaternion.Euler(newAngleX, transform.rotation.eulerAngles.y, 0);

            foreach (Transform barrels in barrelTransform)
            {
                barrels.LookAt(target.transform);
            }
            
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
