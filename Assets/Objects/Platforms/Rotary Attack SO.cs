using UnityEngine;

[CreateAssetMenu(menuName = "Turrets/Rotary Turret")]
public class RotaryAttackSO : ScriptableObject
{
    public float attackSpeed;
    public float delayPerBarrel;
    public float attackRange;
    public float barrelRotationSpeed;
    public GameObject projectilePrefab;
    public LayerMask targetLayer;
}
