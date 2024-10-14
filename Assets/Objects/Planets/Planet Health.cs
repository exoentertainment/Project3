using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlanetHealth : MonoBehaviour, IDamageable
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] PlanetHealthSO planetHealthSO;

    [Header("Components")]
    [SerializeField] Slider healthSlider;

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
    
    private void Update()
    {
        healthSlider.transform.LookAt(Camera.main.transform);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
        
        if(currentHealth <= 0)
            DestroyPlanet();
    }
    
    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth/planetHealthSO.maxHealth;
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
