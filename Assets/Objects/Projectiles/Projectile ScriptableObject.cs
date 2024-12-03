using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Projectiles/Projectile")]
public class IProjectileScriptableObject : ScriptableObject
{
    public int moveSpeed;
    public float damage;
    public float lifeTime;
    public float blastRadius;
    public LayerMask targetLayer;
    public GameObject projectilePrefab;
    public GameObject explodeEffectPrefab;
}
