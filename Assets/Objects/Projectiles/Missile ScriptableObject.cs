using UnityEngine;

[CreateAssetMenu(fileName = "Missile Projectile", menuName = "Projectiles/Missile Projectile")]
public class MissileScriptableObject : ScriptableObject
{
    public int minMoveSpeed;
    public int maxMoveSpeed;
    public float damage;
    public int attackRange;
    public float lifeTime;
    public LayerMask targetLayer;
    public GameObject dischargeEffectPrefab;
    public GameObject explodeEffectPrefab;
}
