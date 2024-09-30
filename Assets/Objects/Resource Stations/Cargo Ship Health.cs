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

    public void TakeDamage(int damage)
    {
        
    }
}
