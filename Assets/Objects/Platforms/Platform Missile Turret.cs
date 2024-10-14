using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class PlatformMissileTurret : MonoBehaviour
{
        #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] TurretSO platformTurretSO;

    [Header("Prefab Object")] [SerializeField]
    GameObject projectilePrefab;

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
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (target == null)
                break;
            
            Vector3 targetVector = target.transform.position - spawnPoint.position;
            targetVector.Normalize();
            float rotateAmountZ = Vector3.Cross(targetVector, spawnPoint.forward).z;
            float rotateAmountX = Vector3.Cross(targetVector, spawnPoint.forward).x;
            float rotateAmountY = Vector3.Cross(targetVector, spawnPoint.forward).y;
            
            float newAngleZ = spawnPoint.rotation.eulerAngles.z + (-rotateAmountZ);
            float newAngleX = spawnPoint.rotation.eulerAngles.x + (-rotateAmountX);
            float newAngleY = spawnPoint.rotation.eulerAngles.y + (-rotateAmountY);
            
            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            //projectile.transform.SetParent(transform.parent.parent);
            //projectile.transform.rotation = Quaternion.Euler(newAngleX, newAngleY, newAngleZ);
            
            yield return new WaitForSeconds(platformTurretSO.delayPerBarrel);
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
            // Vector3 targetVector = target.transform.position - tubeTransforms.position;
            // targetVector.Normalize();
            // // float rotateAmountZ = Vector3.Cross(targetVector, transform.forward).z;
            // float rotateAmountX = Vector3.Cross(targetVector, tubeTransforms.forward).x;
            // // float rotateAmountY = Vector3.Cross(targetVector, tubeTransforms.forward).y;
            //
            // // float newAngleZ = transform.rotation.eulerAngles.z + (-rotateAmountZ);
            // float newAngleX = tubeTransforms.rotation.eulerAngles.x + (-rotateAmountX);
            // // float newAngleY = tubeTransforms.rotation.eulerAngles.y + (-rotateAmountY);
            //
            // tubeTransforms.rotation = Quaternion.Euler(newAngleX, transform.rotation.eulerAngles.y, 0);
            
            missileTubeTransform.LookAt(target.transform);
            
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, platformTurretSO.attackRange);
    }
}
