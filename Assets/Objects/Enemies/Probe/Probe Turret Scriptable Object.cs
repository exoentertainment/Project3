using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Probe Turret Scriptable Object")]
public class ProbeTurretScriptableObject : ScriptableObject
{
    public float attackSpeed;
    public float delayPerBarrel;
    public float attackRange;
    public float barrelRotationSpeed;
    public GameObject projectilePrefab;
    public LayerMask targetLayer;
}
