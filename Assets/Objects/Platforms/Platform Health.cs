using System;
using System.Collections;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlatformHealth : MonoBehaviour, IDamageable
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] PlatformScriptableObject healthSO;
    
    [Header("Components")]
    [SerializeField] Slider healthSlider;
    [SerializeField] private Transform[] explosionPoints;
    
    [Header("Events")]
    [SerializeField] UnityEvent OnDeath;
    
    [Header("Feedbacks")]
    [SerializeField] MMFeedbacks deathFeedback;

    [Header("Variables")] 
    [SerializeField] private float minExplosionForce;
    [SerializeField] private float maxExplosionForce;

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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            OnDeath?.Invoke();
            
            if(CameraManager.instance.IsObjectInView(gameObject.transform))
                AudioManager.instance.PlayPlatformExplosion();
                
            deathFeedback?.PlayFeedbacks();
            Instantiate(healthSO.explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            
            //StartCoroutine(DestroyPlatformRoutine());
        }
    }

    IEnumerator DestroyPlatformRoutine()
    {
        for (int i = 0; i < explosionPoints.Length; i++)
        {
            Instantiate(healthSO.explosionPrefab, explosionPoints[i].position, Quaternion.identity);
            
            if(CameraManager.instance.IsObjectInView(gameObject.transform))
                AudioManager.instance.PlayPlatformExplosion();
                
            deathFeedback?.PlayFeedbacks();
            PushSegmentAway(i);
            
            yield return new WaitForSeconds(healthSO.delayBetweenExplosions);
            
            Instantiate(healthSO.explosionPrefab, explosionPoints[i].position, Quaternion.identity);
            
            yield return new WaitForSeconds(healthSO.delayBetweenExplosions);
        }

        Destroy(gameObject, 2);
    }

    void PushSegmentAway(int segment)
    {
        GetComponent<Rigidbody>().AddExplosionForce(Random.Range(minExplosionForce, maxExplosionForce), transform.position, 500);
        
        Transform[] childTransforms = explosionPoints[segment].gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in childTransforms)
        {
            child.gameObject.AddComponent<Rigidbody>();
            child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(minExplosionForce, maxExplosionForce), transform.position, 500);
        }
    }
    
    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth/healthSO.maxHealth;
    }
}
