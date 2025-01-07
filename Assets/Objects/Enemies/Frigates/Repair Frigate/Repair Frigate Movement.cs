using UnityEngine;
using System.Collections;

public class RepairFrigateMovement : MonoBehaviour
{
    [Header("Scriptable Object")] 
    [SerializeField] private EnemyScriptableObject enemySO;

    [SerializeField] private float repairThreshold;
    
    private bool isFloating = true;
    private GameObject target;
    
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
                FindClosestTarget();
            }

            yield return null;
        }
    }

    void FindClosestTarget()
    {
        float closestEnemy = Mathf.Infinity;
        
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, enemySO.targetLayer);
        
        if (potentialTargets.Length > 0)
        {
            for (int x = 0; x < potentialTargets.Length; x++)
            {
                if (potentialTargets[x].gameObject == gameObject)
                {
                    continue;
                }
                
                float distanceToEnemy =
                    Vector3.Distance(potentialTargets[x].transform.position, transform.position);
                
                if (distanceToEnemy < closestEnemy)
                {
                    potentialTargets[x].TryGetComponent<IRepairable>(out IRepairable repairObject);
                    
                    if (repairObject?.GetHealth() > repairThreshold)
                        continue;
                    
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
            
            float newAngleY = transform.rotation.eulerAngles.y + (-rotateAmountY * enemySO.turnRate * Time.deltaTime);
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
            if(Vector3.Distance(transform.position, target.transform.position) > enemySO.standOffDistance)
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, enemySO.moveSpeed * Time.deltaTime);
        }
    }
}
