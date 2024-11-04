using UnityEngine;
using System.Collections;

public class Boss1Movement : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] int moveSpeed;
    [SerializeField] int turnRate;
    [SerializeField] float standOffDistance;
    [SerializeField] private float planetKillingDistance;
    [SerializeField] float coastDelay;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] private LayerMask planet;

    #endregion
    
    private bool isFloating = true;
    private GameObject target;
    
    private void Start()
    {
        StartCoroutine(FloatShipRoutine());
        CameraManager.instance.ZoomOnBoss(this.transform);
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
            Vector3 targetVector = target.transform.position - transform.position;
            targetVector.Normalize();
            float rotateAmountY = Vector3.Cross(targetVector, transform.forward).y;
            
            float newAngleY = transform.rotation.eulerAngles.y + (-rotateAmountY * turnRate * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, newAngleY, 0);
        }
    }

    void MoveTowardsTarget()
    {
        if (target == null && !isFloating)
        {
            FindClosestTarget();
            return;
        }

        if (!isFloating)
        {
            if(Vector3.Distance(transform.position, target.transform.position) > standOffDistance)
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    public void SwitchTargetToPlanet()
    {
        standOffDistance = planetKillingDistance;
        
        float closestEnemy = Mathf.Infinity;
        
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, planet);
        
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
