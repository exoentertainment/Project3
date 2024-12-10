using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour, IDamageable
{
    #region -- Serialized Fields

    [Header("Scriptable Objects")]
    [SerializeField] AsteroidSO asteroidSO;
    
    [Header("Components")]
    [SerializeField] Rigidbody[] rb;

    #endregion

    private float currentHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = asteroidSO.health;
        
        SetMaterial();
        FindTarget();
        Spin();
        
        Destroy(gameObject, asteroidSO.lifespan);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
    
    void FindTarget()
    {
        float closestEnemy = Mathf.Infinity;
        GameObject target = null;

        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, asteroidSO.targetLayers);

        if (potentialTargets.Length > 0)
        {
            for (int x = 0; x < potentialTargets.Length; x++)
            {
                float distanceToEnemy =
                    Vector3.Distance(potentialTargets[x].transform.position, transform.position);

                if (distanceToEnemy < closestEnemy)
                {
                    closestEnemy = distanceToEnemy;
                    target = potentialTargets[x].gameObject;
                }
            }
            
            transform.LookAt(target.transform);
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
    
    void Move()
    {
        transform.position += transform.forward * (asteroidSO.moveSpeed * Time.deltaTime);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Celestial Body"))
        {
            Instantiate(asteroidSO.explosionPrefab, other.GetContact(0).point, Quaternion.identity);
            
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hit))
            {
                hit.TakeDamage(asteroidSO.damage);
            }
            
            if(CameraManager.instance.IsObjectInView(gameObject.transform))
                AudioManager.instance.PlayLargeExplosion();
                
            Destroy(transform.root.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if(currentHealth <= 0)
            Destroy(gameObject);
    }
}
