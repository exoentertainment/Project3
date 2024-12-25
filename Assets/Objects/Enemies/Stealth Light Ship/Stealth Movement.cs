using UnityEngine;
using System.Collections;

public class StealthMovement : MonoBehaviour
{
    [Header("Scriptable Object")] 
    [SerializeField] private EnemyScriptableObject enemySO;
    
    [Header("Material")]
    [SerializeField] Material visibleMaterial;

    [Header("Components")]
    [SerializeField] MeshRenderer[] meshRenderers;
    
    private bool isFloating = false;
    private bool isStealth = true;
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
        
        if (!isFloating)
        {
            if (Vector3.Distance(transform.position, target.transform.position) >= enemySO.standOffDistance)
            {
                transform.position += transform.forward * enemySO.moveSpeed * Time.deltaTime;
            }
            else
            {
                if (isStealth)
                {
                    isStealth = false;
                    ChangeMaterial();
                    gameObject.layer = LayerMask.NameToLayer("Enemy");
                }
            }
        }
    }

    void ChangeMaterial()
    {
        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.material = visibleMaterial;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemySO.standOffDistance);
    }
}
