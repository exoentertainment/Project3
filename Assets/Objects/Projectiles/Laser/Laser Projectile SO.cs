using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Projectiles/Laser")]
public class LaserProjectileSO : ScriptableObject
{
    public float damage;
    public float damageBonus;
    public LayerMask targetLayer;
}
