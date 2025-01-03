using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

public class EnemyHealth : MonoBehaviour, IDamageable, IRepairable
{
    #region -- SerializeFields --

    [Header("Scriptable Object")] 
    [SerializeField] private EnemyScriptableObject enemySO;

    [Header("Components")]
    [SerializeField] Slider healthSlider;
    
    [Header("Events")]
    [SerializeField] UnityEvent OnDeath;
    
    [Header("Feedbacks")]
    [SerializeField] MMFeedbacks deathFeedback;

    [SerializeField] private bool isFragments;
    
    #endregion

    private float currentHealth;
    private bool isDead;
    private bool isBeingHit;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = enemySO.maxHealth;
    }
    
    private void Update()
    {
        healthSlider.transform.LookAt(Camera.main.transform);
        
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

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;

            if (CameraManager.instance.IsObjectInView(transform))
                AudioManager.instance.PlayEnemySmallExplosion();

            ResourceManager.instance?.IncreaseResources(enemySO.resourceReward);
            Instantiate(enemySO.explosionPrefab, transform.position, Quaternion.identity);

            CameraManager.instance.RemoveTrackingObject();

            deathFeedback?.PlayFeedbacks();

            if (isFragments)
                StartCoroutine(ExplodeShip());

            if (isFragments)
                Destroy(transform.root.gameObject, 1f);
            else
                Destroy(gameObject);
        }
    }

    IEnumerator ExplodeShip()
    {
        Transform[] shipParts = transform.GetComponentsInChildren<Transform>();

        Rigidbody rb;
            
        foreach (Transform part in shipParts)
        {
            part.gameObject.AddComponent<Rigidbody>();
            rb = part.gameObject.GetComponent<Rigidbody>();
            
            rb.AddExplosionForce(300, transform.position, 50);
            rb.AddExplosionForce(300, part.position, 50);
            Instantiate(enemySO.explosionPrefab, part.position, Quaternion.identity);

            yield return new WaitForSeconds(.1f);
        }
    }
    
    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth/enemySO.maxHealth;
    }

    public float GetHealth()
    {
        return currentHealth / enemySO.maxHealth;
    }

    public void RepairHealth(int value)
    {
        currentHealth += value;
        UpdateHealthBar();
        
        if(currentHealth > enemySO.maxHealth)
            currentHealth = enemySO.maxHealth;
    }
}
