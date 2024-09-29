using UnityEngine;

[CreateAssetMenu(fileName = "Cargo Ship", menuName = "Cargo Ship")]
public class CargoShipScriptableObject : ScriptableObject
{
    public int moveSpeed;
    public int moveDelay;
}
