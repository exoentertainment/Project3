using System;
using MoreMountains.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidBeltObject : MonoBehaviour, IDamageable
{
    #region -- Serialized Fields
    
    [Header("Scriptable Objects")]
    [SerializeField] AsteroidSO asteroidSO;
    
    [Header("Variables")]
    [SerializeField] int minYOffset;
    [SerializeField] int maxYOffset;
    [SerializeField] private int minOrbitSpeed;
    [SerializeField] private int maxOrbitSpeed;
    
    [Header("Components")]
    [SerializeField] Rigidbody[] rb;
    [SerializeField] MMAutoRotate autoRotate;

    #endregion

    private float currentHealth;
    
    void Start()
    {
        currentHealth = asteroidSO.health;
        
        SetMaterial();
        SetYOffset();
        SetOrbitSpeed();
        Spin();
    }

    void SetYOffset()
    {
        autoRotate.OrbitCenterOffset.y = Random.Range(minYOffset, maxYOffset);
    }

    void SetOrbitSpeed()
    {
        autoRotate.OrbitRotationSpeed = Random.Range(minOrbitSpeed, maxOrbitSpeed);
    }
    
    void SetMaterial()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        int materialNum = Random.Range(0, asteroidSO.materials.Length);

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = asteroidSO.materials[materialNum];
        }
    }
    
    void Spin()
    {
        foreach (Rigidbody rigidBody in rb)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rigidBody.AddTorque(randomDirection * 10);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Celestial Body") || other.gameObject.layer == LayerMask.NameToLayer("Weapon Platform"))
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable target))
            {
                target.TakeDamage(asteroidSO.damage);
                Instantiate(asteroidSO.explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
