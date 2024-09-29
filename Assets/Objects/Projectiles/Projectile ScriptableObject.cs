using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Projectiles/Projectile")]
public class IProjectileScriptableObject : ScriptableObject
{
    public int moveSpeed;
    public int damage;
    public float lifeTime;
}
