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

    [Header("Components")]
    [SerializeField] Slider healthSlider;
    [SerializeField] private Transform[] explosionPoints;
    
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
    private float currentHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        onSpawn?.Invoke();
        onDeath.AddListener(GameManager.instance.LoadNextLevelButton);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if ((currentHealth/maxHealth) <= lowHealthLimit && !isLowHealth)
        {
            isLowHealth = true;
            onLowHealth?.Invoke();
        }

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            //CameraManager.instance.ZoomOnBoss(transform);
            //PushSegmentAway();
            DestroyShip();
        }
    }
    
    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth/maxHealth;
    }

    void DestroyShip()
    {
        if(CameraManager.instance.IsObjectInView(transform))
            AudioManager.instance.PlayEnemySmallExplosion();

        foreach (GameObject explosion in explosionPrefabs)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        
        GameManager.instance.LoadNextLevelButton();
        Destroy(gameObject);
    }
    
    void PushSegmentAway()
    {
        foreach (Transform child in explosionPoints)
        {
            child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(150, 300), transform.position, 10);
        }
    }
}
