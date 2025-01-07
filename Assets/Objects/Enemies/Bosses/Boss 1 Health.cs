using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Boss1Health : MonoBehaviour, IDamageable
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] private float maxHealth;
    [SerializeField] private float lowHealthLimit;
    [SerializeField] private int numExplosions;
    [SerializeField] float delayBetweenExplosions;

    [Header("Components")]
    [SerializeField] Slider healthSlider;
    
    [Header("Events")] 
    [SerializeField] private UnityEvent onSpawn;
    [SerializeField] private UnityEvent onLowHealth;
    [SerializeField] private UnityEvent onDeath;

    [FormerlySerializedAs("explosionPrefab")]
    [Header("Prefabs")] 
    [SerializeField] private GameObject[] explosionPrefabs;

    #endregion

    private bool isDead;
    private bool isLowHealth;
    private bool isBeingHit;
    private float currentHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        onSpawn?.Invoke();
        onDeath.AddListener(GameManager.instance.LoadNextLevelButton);
    }

    private void Update()
    {
        if(isBeingHit)
            isBeingHit = false;
    }

    public void TakeDamage(float damage)
    {
        if (!isBeingHit)
        {
            isBeingHit = true;
            currentHealth -= damage;
            UpdateHealthBar();
        }

        if ((currentHealth/maxHealth) <= lowHealthLimit && !isLowHealth)
        {
            isLowHealth = true;
            onLowHealth?.Invoke();
        }

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            onDeath?.Invoke();
            StartCoroutine(DestroyShip());
        }
    }
    
    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth/maxHealth;
    }

    IEnumerator DestroyShip()
    {
        if(CameraManager.instance.IsObjectInView(transform))
            AudioManager.instance.PlayEnemySmallExplosion();

        for (int i = 0; i < numExplosions; i++)
        {
            int randomExplosion = Random.Range(0, explosionPrefabs.Length);
            BoxCollider collider = GetComponent<BoxCollider>();
            Vector3 randomPosition = new Vector3(Random.Range(collider.bounds.min.x, collider.bounds.max.x), Random.Range(collider.bounds.min.y, collider.bounds.max.y), Random.Range(collider.bounds.min.z, collider.bounds.max.z));
            Instantiate(explosionPrefabs[randomExplosion], randomPosition, Quaternion.identity);
            yield return new WaitForSeconds(delayBetweenExplosions);
        }
        
        yield return new WaitForSeconds(2);
        GameManager.instance.LoadNextLevelButton();
        Destroy(gameObject);
    }
}
