using System;
using UnityEngine;
using MoreMountains.Feedbacks;

public class MainMassDriver : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] private Transform dischargeSpawnPoint;
    [SerializeField] private float fireRate;
    [SerializeField] MMFeedbacks firingFeedback;
    [SerializeField] private LayerMask targetLayer;

    [Header("Prefabs")]
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] GameObject dischargePrefab;
    
    #endregion

    float lastFireTime;
    GameObject target;

    private void Start()
    {
        lastFireTime = Time.time;
        
        FindNearestPlanet();
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            FireMassDriver();
        }
    }

    void FindNearestPlanet()
    {
        if (target == null)
        {
            float closestEnemy = Mathf.Infinity;

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, targetLayer);

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

    void FireMassDriver()
    {
        if((Time.time - lastFireTime) >= fireRate)
        {
            lastFireTime = Time.time;
            
            if (CameraManager.instance.IsObjectInView(transform))
            {
                AudioManager.instance.PlayMassDriverBossWeaponFire();
                firingFeedback?.PlayFeedbacks();
            }
            
            Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
            Instantiate(dischargePrefab, dischargeSpawnPoint.transform.position, Quaternion.identity);
        }
    }
}
