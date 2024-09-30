using UnityEngine;

[CreateAssetMenu(fileName = "New Resource Station", menuName = "Resource Station")]
public class ResourceStationScriptableObject : ScriptableObject
{
    public float cargoShipSpawnTime;
    public int maxHealth;
}
