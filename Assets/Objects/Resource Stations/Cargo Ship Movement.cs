using System;
using System.Collections;
using UnityEngine;

public class CargoShipMovement : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")] 
    [SerializeField] private CargoShipScriptableObject cargoShipSO;
    
    [Header("Variables")]
    [SerializeField] LayerMask resourceStationLayerMask;

    #endregion

    private GameObject originStation;
    private GameObject target;

    private bool isFloating = true;

    private void Start()
    {
        StartCoroutine(FloatShipRoutine());
    }

    private void Update()
    {
        if (Time.timeScale == 1 && originStation != null)
        {
            Rotate();
            MoveTowardsTarget();
        }
    }

    public void SetOriginStation(GameObject station)
    {
        originStation = station;
        transform.rotation = originStation.transform.rotation;
        
        //FindNearestStation();
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
    
    void MoveTowardsTarget()
    {
        if (target == null && !isFloating)
        {
            FindNearestStation();
            return;
        }

        if (!isFloating)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, cargoShipSO.moveSpeed * Time.deltaTime);
        }
    }
    
    IEnumerator FloatShipRoutine()
    {
        float floatTime = 0;

        while (isFloating)
        {
            transform.position += transform.forward * (cargoShipSO.moveSpeed * Time.deltaTime);
            
            floatTime += Time.deltaTime;
            if (floatTime >= cargoShipSO.moveDelay)
            {
                isFloating = false;
                FindNearestStation();
            }

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject != originStation)
        {
            ResourceManager.instance.IncreaseResources(cargoShipSO.resourceAmount);
            Destroy(gameObject);
        }
    }
}
