using System;
using UnityEngine;

public class ResourceStation : MonoBehaviour
{
    #region -- Serialized Fields --
    
    [Header("Prefabs")]
    [SerializeField] GameObject cargoShipPrefab;
    
    [Header("Components")]
    [SerializeField] Transform cargoShipSpawnPoint;

    [Header("Scriptable Object")] 
    [SerializeField] private ResourceStationScriptableObject resourceStationSO;
    
    [SerializeField] LayerMask resourceStationLayerMask;
    #endregion

    private float lastSpawnTime;

    private void Start()
    {
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        SpawnCargoShip();
    }

    void SpawnCargoShip()
    {
        if ((Time.time - lastSpawnTime) >= resourceStationSO.cargoShipSpawnTime)
        {
            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, resourceStationLayerMask);
            Debug.Log(potentialTargets.Length);

            if (potentialTargets.Length > 1)
            {
                GameObject cargoShip =
                    Instantiate(cargoShipPrefab, cargoShipSpawnPoint.position, Quaternion.identity);
                cargoShip.GetComponent<CargoShipMovement>().SetOriginStation(gameObject);

                lastSpawnTime = Time.time;
            }
        }
    }
}
