using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlatformHealth : MonoBehaviour, IDamageable
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] PlatformScriptableObject healthSO;
    
    [Header("Components")]
    [SerializeField] Slider healthSlider;

    #endregion
    
    float currentHealth; 
    bool isDestroyed = false;

    private void Start()
    {
        currentHealth = healthSO.maxHealth;
    }

    private void Update()
    {
        healthSlider.transform.LookAt(Camera.main.transform);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            StartCoroutine(DestroyPlatformRoutine());
        }
    }

    IEnumerator DestroyPlatformRoutine()
    {
        Transform[] stationPoints = transform.GetComponentsInChildren<Transform>();
        
        for (int i = 0; i < stationPoints.Length/2; i++)
        {
            Instantiate(healthSO.explosionPrefab, stationPoints[Random.Range(0, stationPoints.Length)].position, Quaternion.identity);
            yield return new WaitForSeconds(healthSO.delayBetweenExplosions);
        }
        
        yield return new WaitForSeconds(2f);
        
        Destroy(gameObject);
    }

    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth/healthSO.maxHealth;
    }
}
