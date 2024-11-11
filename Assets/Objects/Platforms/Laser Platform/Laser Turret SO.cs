using UnityEngine;

[CreateAssetMenu(fileName = "Laser Turret", menuName = "Turrets/Laser Turret")]
public class LaserTurretSO : ScriptableObject
{
    public float rechargeTime;
    public float range;
    public LayerMask targetLayer;
}
