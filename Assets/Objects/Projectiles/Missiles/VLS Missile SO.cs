using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/VLS Missile")]
public class VLSMissileSO : ScriptableObject
{
    public int moveSpeed;
    public float coastDuration;
    public int coastSpeed;
    public float damage;
    public int attackRange;
    public float lifeTime;
    public LayerMask targetLayer;
    public GameObject projectilePrefab;
    public GameObject dischargeEffectPrefab;
    public GameObject explodeEffectPrefab;
}
