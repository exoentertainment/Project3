using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class Boss1Movement : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] int moveSpeed;
    [SerializeField] float standOffDistance;
    [SerializeField] private float planetKillingDistance;
    [SerializeField] float coastDelay;
    [SerializeField] LayerMask targetLayer;
    [FormerlySerializedAs("planet")] [SerializeField] private LayerMask planetLayer;

    #endregion
    
    private bool isFloating = true;
    private GameObject target;
    
    private void Start()
    {
        StartCoroutine(FloatShipRoutine());
        CameraManager.instance.ZoomOnBoss(transform);
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
            transform.position += transform.forward * (moveSpeed * Time.deltaTime);
            
            floatTime += Time.deltaTime;
            if (floatTime >= coastDelay)
            {
                isFloating = false;
                FindClosestTarget();
            }

            yield return null;
        }
    }

    void FindClosestTarget()
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
            FindClosestTarget();
            return;
        }

        if (!isFloating && Vector3.Distance(transform.position, target.transform.position) > standOffDistance)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    public void SwitchTargetToPlanet()
    {
        standOffDistance = planetKillingDistance;
        
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
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, standOffDistance);
    }
}
