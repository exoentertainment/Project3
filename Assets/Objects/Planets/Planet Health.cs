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
            
            Instantiate(planetHealthSO.explosionPrefab, explosionPoints[Random.Range(0, explosionPoints.Length)].position, Quaternion.identity);

            yield return new WaitForSeconds(planetHealthSO.delayPerExplosion);
        }
        
        Destroy(gameObject);
    }
}
