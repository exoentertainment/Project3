using System;
using UnityEngine;

public class CargoShip : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")] 
    [SerializeField] private CargoShipScriptableObject cargoShipSO;
    
    [Header("Variables")]
    [SerializeField] LayerMask resourceStationLayerMask;

    #endregion

    private GameObject originStation;
    private GameObject target;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Time.timeScale == 1 && originStation != null)
        {
            Rotate();
            Move();
        }
    }

    public void SetOriginStation(GameObject station)
    {
        originStation = station;
        
        FindNearestStation();
    }

    void FindNearestStation()
    {
        if (target == null)
        {
            float closestEnemy = Mathf.Infinity;

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, resourceStationLayerMask);

            if (potentialTargets.Length > 0)
            {
                for (int x = 0; x < potentialTargets.Length; x++)
                {
                    float distanceToEnemy =
                        Vector3.Distance(potentialTargets[x].transform.position, transform.position);

                    if (distanceToEnemy < closestEnemy && potentialTargets[x].gameObject != originStation)
                    {
                        closestEnemy = distanceToEnemy;
                        target = potentialTargets[x].gameObject;
                    }
                }
            }
        }
    }

    void Rotate()
    {
        if (target != null)
        {
            Vector3 targetVector = target.transform.position - transform.position;
            targetVector.Normalize();
            float rotateAmountY = Vector3.Cross(targetVector, transform.forward).y;

            float newAngleY = transform.rotation.eulerAngles.y - rotateAmountY;
            transform.rotation = Quaternion.Euler(0, newAngleY, 0);
        }
    }
    
    void Move()
    {
        
    }
}
