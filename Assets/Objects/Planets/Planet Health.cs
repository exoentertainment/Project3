using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetHealth : MonoBehaviour, IDamageable
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] PlanetHealthSO planetHealthSO;

    [Header("Components")] 
    [SerializeField] private Transform[] explosionPoints;

    #endregion

    private int currentHealth;
    SphereCollider sphereCollider;
    MeshRenderer meshRenderer;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        
        Debug.Log(sphereCollider.radius);
        Debug.Log(meshRenderer.bounds.extents.magnitude);
    }

    private void Start()
    {
        currentHealth = planetHealthSO.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if(currentHealth <= 0)
            DestroyPlanet();
    }

    void DestroyPlanet()
    {
        StartCoroutine(DestroyPlanetCoroutine());
        gameObject.layer = LayerMask.NameToLayer("Destroyed Planet");
    }

    IEnumerator DestroyPlanetCoroutine()
    {
        float timer = 0f;

        while (timer < planetHealthSO.explosionDuration)
        {
            timer += Time.deltaTime;
            
            Vector3 pos = transform.position + Random.onUnitSphere * (sphereCollider.radius * transform.lossyScale.x);
            pos.x += sphereCollider.radius;
            pos.y += sphereCollider.radius;
            pos.z += sphereCollider.radius;
            
            Instantiate(planetHealthSO.explosionPrefab, pos, Quaternion.identity);

            yield return new WaitForSeconds(planetHealthSO.delayPerExplosion);
        }
        
        Destroy(gameObject);
    }
}
