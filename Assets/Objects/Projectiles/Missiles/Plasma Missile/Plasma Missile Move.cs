using UnityEngine;

public class PlasmaMissileMove : MonoBehaviour
{
    #region -- Serialized Fields --
    
    [Header("Scriptable Object")] 
    [SerializeField] private PlasmaMissileSO missileSO;

    #endregion

    private GameObject target;
    
    private void Start()
    {
        if(CameraManager.instance.IsObjectInView(transform))
            AudioManager.instance.PlayPlasmaMissileTurretSound();
        
        FindTarget();
        Destroy(gameObject, missileSO.lifeTime);
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            Move();
        }
    }

    void FindTarget()
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
    
    void Move()
    {
        if (target != null)
        {
            transform.LookAt(target.transform, transform.up);
        }
        else
        {
            FindTarget();
        }
        
        transform.position += transform.forward * (missileSO.moveSpeed * Time.deltaTime);
    }
}
