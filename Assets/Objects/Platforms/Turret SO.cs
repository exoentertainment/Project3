using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "Turrets/Turret")]
public class TurretSO : ScriptableObject
{
    public float attackSpeed;
    public float delayPerBarrel;
    public float attackRange;
    public LayerMask targetLayer;
    public GameObject projectilePrefab;
    public GameObject dischargePrefab;
}
