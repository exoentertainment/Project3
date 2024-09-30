using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformHealth : MonoBehaviour, IDamageable
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] PlatformHealthSO healthSO;

    #endregion
    
    float currentHealth;

    private void Start()
    {
        currentHealth = healthSO.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if(currentHealth <= 0)
                ExplodePlatform();
    }

    void ExplodePlatform()
    {
        
    }
}
