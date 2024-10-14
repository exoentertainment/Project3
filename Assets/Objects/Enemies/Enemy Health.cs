using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    #region -- SerializeFields --

    [Header("Scriptable Object")] 
    [SerializeField] private EnemyScriptableObject enemySO;

    [Header("Components")]
    [SerializeField] Slider healthSlider;
    
    #endregion

    private float currentHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = enemySO.maxHealth;
    }
    
    private void Update()
    {
        healthSlider.transform.LookAt(Camera.main.transform);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Instantiate(enemySO.explosionPrefab, transform.position, Quaternion.identity);
            Destroy(transform.root.gameObject);
        }
    }
    
    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth/enemySO.maxHealth;
    }
}
