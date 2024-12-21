using System;
using UnityEngine;

public class MassDriverAsteroid : MonoBehaviour
{
    [SerializeField] AsteroidSO asteroidSO;

    private void FixedUpdate()
    {
        if (Time.timeScale == 1)
        {
            Move();
        }
    }

    void Move()
    {
        transform.position += transform.forward * (asteroidSO.moveSpeed * Time.deltaTime);
    }
    
    private void OnCollisionEnter(Collision other)
    {   
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hit))
        {
            hit.TakeDamage(asteroidSO.damage);
            Instantiate(asteroidSO.explosionPrefab, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
