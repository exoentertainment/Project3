using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    #region -- SerializeFields --

    [Header("Scriptable Object")] 
    [SerializeField] private EnemyScriptableObject enemySO;

    #endregion

    private float currentHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = enemySO.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        Debug.Log(currentHealth);
        
        if(currentHealth <= 0)
            Destroy(gameObject);
    }
}
