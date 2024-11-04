using System;
using UnityEngine;

public class CargoShipHealth : MonoBehaviour, IDamageable
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")] 
    [SerializeField] private CargoShipScriptableObject cargoShipSO;

    #endregion

    private float currentHealth;

    private void Start()
    {
        currentHealth = cargoShipSO.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            if(CameraManager.instance.IsObjectInView(transform))
                AudioManager.instance.PlayEnemySmallExplosion();
            
            Instantiate(cargoShipSO.explodePrefab, transform.position, Quaternion.identity);
            Destroy(transform.root.gameObject);
        }
    }
}
