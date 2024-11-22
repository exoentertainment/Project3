using UnityEngine;

public class ShieldControl : MonoBehaviour, IDamageable
{
    [SerializeField] ParticleSystem shield;
    [SerializeField] private float shieldHP;
    
    public void TakeDamage(float damage)
    {
        shield.Emit(1);
        
        shieldHP -= damage;
        
        if (shieldHP <= 0)
            Destroy(gameObject);
    }
}
