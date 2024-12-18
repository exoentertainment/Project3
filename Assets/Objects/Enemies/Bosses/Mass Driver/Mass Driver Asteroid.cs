using System;
using UnityEngine;

public class MassDriverAsteroid : MonoBehaviour, IDamageable
{
    [SerializeField] AsteroidSO asteroidSO;

    private float currentHealth;

    private void Start()
    {
        currentHealth = asteroidSO.health;
    }

    public void TakeDamage(float damage)
    {
        
    }
}
