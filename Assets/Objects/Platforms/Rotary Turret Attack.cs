using UnityEngine;
using System.Collections;

public class RotaryTurretAttack : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] RotaryAttackSO turretSO;

    [Header("Components")] 
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform barrelMountTransform;
    [SerializeField] private Transform barrelTransform;
    
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
        foreach (Transform spawnPoint in spawnPoints)
        {
            Vector3 targetVector = target.transform.position - spawnPoint.position;
            targetVector.Normalize();
            float rotateAmountZ = Vector3.Cross(targetVector, spawnPoint.forward).z;
            float rotateAmountX = Vector3.Cross(targetVector, spawnPoint.forward).x;
            float rotateAmountY = Vector3.Cross(targetVector, spawnPoint.forward).y;
            
            float newAngleZ = spawnPoint.rotation.eulerAngles.z + (-rotateAmountZ);
            float newAngleX = spawnPoint.rotation.eulerAngles.x + (-rotateAmountX);
            float newAngleY = spawnPoint.rotation.eulerAngles.y + (-rotateAmountY);
            
            GameObject projectile = Instantiate(turretSO.projectilePrefab, spawnPoint.position, Quaternion.identity);
            projectile.transform.rotation = Quaternion.Euler(barrelMountTransform.eulerAngles.x, barrelMountTransform.eulerAngles.y, barrelMountTransform.eulerAngles.z);
            //projectile.transform.rotation = Quaternion.Euler(newAngleX, newAngleY, newAngleZ);
            
            yield return new WaitForSeconds(turretSO.delayPerBarrel);
        }
        // for (int x = 0; x < barrelTransform.Length; x++)
        // {
        //     Vector3 targetVector = target.transform.position - barrelTransform[x].position;
        //     targetVector.Normalize();
        //     float rotateAmountZ = Vector3.Cross(targetVector, barrelTransform[x].forward).z;
        //     float rotateAmountX = Vector3.Cross(targetVector, barrelTransform[x].forward).x;
        //     float rotateAmountY = Vector3.Cross(targetVector, barrelTransform[x].forward).y;
        //     
        //     float newAngleZ = barrelTransform[x].rotation.eulerAngles.z + (-rotateAmountZ);
        //     float newAngleX = barrelTransform[x].rotation.eulerAngles.x + (-rotateAmountX);
        //     float newAngleY = barrelTransform[x].rotation.eulerAngles.y + (-rotateAmountY);
        //     
        //     GameObject projectile = Instantiate(turretSO.projectilePrefab, spawnPoints[x].position, Quaternion.identity);
        //     projectile.transform.SetParent(transform.parent.parent);
        //     projectile.transform.rotation = Quaternion.Euler(newAngleX, newAngleY, newAngleZ);
        //     
             // yield return new WaitForSeconds(turretSO.delayPerBarrel);
        // }
    }
    
    void RotateTurret()
    {
        Vector3 targetVector = target.transform.position - transform.position;
        targetVector.Normalize();

        float rotateAmountY = Vector3.Cross(targetVector, transform.forward).y;
        float newAngleY = transform.rotation.eulerAngles.y + (-rotateAmountY);
        
        transform.rotation = Quaternion.Euler(0, newAngleY, transform.rotation.eulerAngles.z);
    }

    void RotateBarrel()
    {
        barrelMountTransform.LookAt(target.transform);
        
        float newRotationZ = barrelTransform.rotation.eulerAngles.z + (turretSO.barrelRotationSpeed * Time.deltaTime);
        barrelTransform.rotation = Quaternion.Euler(barrelTransform.rotation.eulerAngles.x, barrelTransform.rotation.eulerAngles.y, newRotationZ);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, turretSO.attackRange);
    }
}
