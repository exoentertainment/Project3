using UnityEngine;
using System.Collections;

public class KamikazeMove : MonoBehaviour
{
    [Header("Scriptable Object")] 
    [SerializeField] private KamikazeSO enemySO;
    //needs eventual scriptable object to hold these serialized fields

    private bool isFloating = true;
    private GameObject target;
    
    private void Start()
    {
        FindClosestTarget();
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
            // Vector3 targetVector = target.transform.position - transform.position;
            // targetVector.Normalize();
            // float rotateAmountY = Vector3.Cross(targetVector, transform.forward).y;
            //
            // float newAngleY = transform.rotation.eulerAngles.y + (-rotateAmountY * enemySO.turnRate * Time.deltaTime);
            // transform.rotation = Quaternion.Euler(0, newAngleY, 0);
            
            transform.LookAt(target.transform);
        }
    }

    void MoveTowardsTarget()
    {
        if (1 << target.layer != enemySO.targetLayer && !isFloating)
        {
            FindClosestTarget();
            return;
        }

        if (!isFloating && 1 << target.layer == enemySO.targetLayer)
        {
            // if(Vector3.Distance(transform.position, target.transform.position) > enemySO.accelerationDistance)
            //     transform.position = Vector3.MoveTowards(transform.position, target.transform.position, enemySO.moveSpeed * Time.deltaTime);
            // else
            // {
            //     transform.position = Vector3.MoveTowards(transform.position, target.transform.position, enemySO.kamikazeSpeed * Time.deltaTime);
            // }
            
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, enemySO.moveSpeed * Time.deltaTime);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemySO.accelerationDistance);
    }
}
