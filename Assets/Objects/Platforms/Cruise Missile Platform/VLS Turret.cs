using UnityEngine;
using System.Collections;

public class VLSTurret : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] TurretSO platformTurretSO;
    [SerializeField] VLSMissileSO missileSO;

    [Header("Components")] 
    [SerializeField] private Transform[] spawnPoints;

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
            Fire();
        }
    }

    void SearchForTarget()
    {
        if (target == null)
        {
            float closestEnemy = Mathf.Infinity;

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, missileSO.attackRange, missileSO.targetLayer);

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
            
            if(CameraManager.instance.IsObjectInView(transform))
                AudioManager.instance.PlayCruiseMissileTurretSound();
            
            Instantiate(missileSO.dischargeEffectPrefab, spawnPoint.position, transform.rotation);    
            GameObject projectile = Instantiate(missileSO.projectilePrefab, spawnPoint.position, Quaternion.Euler(-90, 0, 0));
            
            yield return new WaitForSeconds(platformTurretSO.delayPerBarrel);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, missileSO.attackRange);
    }
}
