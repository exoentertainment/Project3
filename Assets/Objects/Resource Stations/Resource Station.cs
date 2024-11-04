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
    
    #endregion

    private float lastSpawnTime;

    private void Start()
    {
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            SpawnCargoShip();
        }
    }

    void SpawnCargoShip()
    {
        if ((Time.time - lastSpawnTime) >= resourceStationSO.cargoShipSpawnTime)
        {

            GameObject cargoShip = Instantiate(cargoShipPrefab, cargoShipSpawnPoint.position, Quaternion.identity);
            cargoShip.transform.SetParent(transform.parent);
            cargoShip.GetComponent<CargoShipMovement>().SetOriginStation(this.gameObject);
            
            lastSpawnTime = Time.time;
        }
    }
}
