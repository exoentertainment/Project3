using System;
using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using Random = UnityEngine.Random;

public class AsteroidGrabberMovement : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")] 
    [SerializeField] private EnemyScriptableObject enemySO;
    //needs eventual scriptable object to hold these serialized fields

    [Header("Variables")]
    [SerializeField] private LayerMask planetLayer;
    [SerializeField] private float asteroidHaulSpeedModified = 1;
    
    #endregion


    private bool isFloating = false;
    private GameObject target;
    private float currentAsteroidHaulSpeedModified = 1;
    
    private void Start()
    {
        StartCoroutine(FloatShipRoutine());
    }

    private void Update()
    {
        RotateTowardsTarget();
        MoveTowardsTarget();
    }

    IEnumerator FloatShipRoutine()
    {
        float floatTime = 0;

        while (isFloating)
        {
            transform.position += transform.forward * (enemySO.moveSpeed * Time.deltaTime);
            
            floatTime += Time.deltaTime;
            if (floatTime >= enemySO.coastDelay)
            {
                isFloating = false;
                FindClosestAsteroid();
            }

            yield return null;
        }
    }

    void FindClosestAsteroid()
    {
        float closestEnemy = Mathf.Infinity;
        
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, enemySO.targetLayer);
        
        if (potentialTargets.Length > 0)
        {
            target = potentialTargets[Random.Range(0, potentialTargets.Length)].gameObject;
        }
        else
        {
            FindClosestPlanet();
        }
    }

    public void FindClosestPlanet()
    {
        float closestEnemy = Mathf.Infinity;
        
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, planetLayer);
        
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

            currentAsteroidHaulSpeedModified = asteroidHaulSpeedModified;
        }
    }
    
    void RotateTowardsTarget()
    {
        if (target != null && !isFloating)
        {
            transform.LookAt(target.transform);
        }
    }

    void MoveTowardsTarget()
    {
        if (target == null && !isFloating)
        {
            FindClosestAsteroid();
            return;
        }

        if (!isFloating)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position,
                    (enemySO.moveSpeed/currentAsteroidHaulSpeedModified) * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.transform.position) <= 10 && target.layer != LayerMask.NameToLayer("Celestial Body"))
            {
                GrabAsteroid();
                FindClosestPlanet();
            }
        }
    }

    void GrabAsteroid()
    {
        target.transform.SetParent(transform);
        target.GetComponent<MMAutoRotate>().enabled = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Celestial Body"))
        {
            Instantiate(enemySO.explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemySO.standOffDistance);
    }
}
