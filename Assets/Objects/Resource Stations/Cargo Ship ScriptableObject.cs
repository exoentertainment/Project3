using UnityEngine;

[CreateAssetMenu(fileName = "Cargo Ship", menuName = "Cargo Ship")]
public class CargoShipScriptableObject : ScriptableObject
{
    public int moveSpeed;
    public float moveDelay;
    public int maxHealth;
    public int resourceAmount;
    public GameObject explodePrefab;
}
