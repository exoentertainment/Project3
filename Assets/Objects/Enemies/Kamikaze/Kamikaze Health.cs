using System;
using UnityEngine;

public class KamikazeHealth : MonoBehaviour, IDamageable
{
    #region -- SerializeFields --

    [Header("Scriptable Object")] 
    [SerializeField] private KamikazeSO enemySO;

    #endregion

    private float currentHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = enemySO.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            Instantiate(enemySO.explosionPrefab, transform.position, Quaternion.identity);
            Destroy(transform.root.gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("destroy ship");
    }
}
