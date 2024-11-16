using UnityEngine;

[CreateAssetMenu(fileName = "Missile Projectile", menuName = "Projectiles/Plasma Missile")]
public class PlasmaMissileSO : ScriptableObject
{
    public int moveSpeed;
    public float damage;
    public float blastRadius;
    public int attackRange;
    public float lifeTime;
    public LayerMask targetLayer;
    public GameObject explodeEffectPrefab;
}
