using System;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] IProjectileScriptableObject projectileSO;

    private void Start()
    {
        SpawnDischargeEffect();
        Destroy(gameObject, projectileSO.lifeTime);
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            Move();
        }
    }

    void Move()
    {
        transform.position += transform.forward * (projectileSO.moveSpeed * Time.deltaTime);
    }

    void SpawnDischargeEffect()
    {
        Instantiate(projectileSO.dischargeEffectPrefab, transform.position, transform.rotation);    
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (1 << other.gameObject.layer == projectileSO.targetLayer)
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hit))
            {
                Debug.Log(other.gameObject.name);
                hit.TakeDamage(projectileSO.damage);
            }
        }
        
        Destroy(gameObject);
    }
}
