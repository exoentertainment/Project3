using System;
using UnityEngine;

public class RepairFriendly : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] private int repairAmount;
    [SerializeField] float repairTime;
    [SerializeField] private float repairThreshhold;
    [SerializeField] private float repairDistance;
    [SerializeField] LayerMask targetLayer;

    [Header("Components")]
    [SerializeField] ParticleSystem repairParticles;
    
    #endregion
    
    float lastRepairTime;
    GameObject target;

    private void Start()
    {
        lastRepairTime = Time.time;
    }

    private void Update()
    {
        FindTarget();
        RepairTarget();
    }

    void FindTarget()
    {
        if (target == null || Vector3.Distance(transform.position, target.transform.position) > repairDistance)
        {
            float closestEnemy = Mathf.Infinity;

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, repairDistance, targetLayer);

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
                        closestEnemy = distanceToEnemy;
                        target = potentialTargets[x].gameObject;
                    }
                }
            }
        }
    }

    void RepairTarget()
    {
        if(target != null)
            if ((Time.time - lastRepairTime) > repairTime)
            { 
                lastRepairTime = Time.time;

                if (target.TryGetComponent<IRepairable>(out IRepairable repairTarget))
                {
                    if (repairTarget.GetHealth() < repairThreshhold)
                    {
                        repairParticles.gameObject.transform.LookAt(target.transform);
                        repairParticles.Play();

                        repairTarget.RepairHealth(repairAmount);
                    }

                    if (repairTarget.GetHealth() > repairThreshhold)
                        target = null;
                }
            }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, repairDistance);
    }
}
