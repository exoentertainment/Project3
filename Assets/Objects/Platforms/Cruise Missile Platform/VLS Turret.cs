using UnityEngine;
using System.Collections;

public class VLSTurret : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] TurretSO platformTurretSO;

    [Header("Components")] 
    [SerializeField] private Transform[] spawnPoints;

    #endregion

    float lastFireTime;
    GameObject target;

    private void Start()
    {
        lastFireTime = Time.time;
        SearchForTarget();
    }

    private void Update()
    {
        Fire();
    }

    void SearchForTarget()
    {
            float closestEnemy = Mathf.Infinity;

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, platformTurretSO.attackRange, platformTurretSO.targetLayer);

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
        if ((Time.time - lastFireTime) >= platformTurretSO.attackSpeed)
        {
            if (target != null)
            {
                StartCoroutine(FireRoutine());
            
                lastFireTime = Time.time;
            }
            else
            {
                SearchForTarget();
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
            
            Instantiate(platformTurretSO.projectilePrefab, spawnPoint.position, Quaternion.Euler(-90, 0, 0));
            
            yield return new WaitForSeconds(platformTurretSO.delayPerBarrel);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, platformTurretSO.attackRange);
    }
}
