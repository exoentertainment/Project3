using System;
using System.Collections;
using UnityEngine;

public class Boss1SpecialWeapon : MonoBehaviour
{
    #region --Serialized Fields--

    [Header("Variables")] 
    [SerializeField] private float chargingTime;
    [SerializeField] private float damage;
    [SerializeField] float rateOfFire;
    [SerializeField] private float chargingEffectGrowthRate;

    [Header("Components")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject chargingEffect;
    
    [Header("Prefabs")]
    [SerializeField] GameObject projectilePrefab;
    
    #endregion

    private float lastFireTime;

    private void Start()
    {
        lastFireTime = Time.time;
    }

    private void Update()
    {
        if(gameObject.activeSelf)
            FireWeapon();
    }

    void FireWeapon()
    {
        if (Time.timeScale == 1)
        {
            if (Time.time - lastFireTime >= rateOfFire)
            {
                StartCoroutine(ChargingRoutine());
                lastFireTime = Time.time;
            }
        }
    }
    
    IEnumerator ChargingRoutine()
    {
        float currentChargingTime = 0;
        chargingEffect.SetActive(true);
        
        if(CameraManager.instance.IsObjectInView(transform))
            AudioManager.instance.PlayPlasmaBossWeaponCharging();

        while (true)
        {
            currentChargingTime += Time.deltaTime;
            
            chargingEffect.transform.localScale +=  chargingEffect.transform.localScale * (chargingEffectGrowthRate * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();

            if (currentChargingTime >= chargingTime)
                break;
        }

        chargingEffect.transform.localScale = Vector3.one;
        chargingEffect.SetActive(false);
        
        if(CameraManager.instance.IsObjectInView(transform))
            AudioManager.instance.PlayPlasmaBossWeaponFire();
        
        Instantiate(projectilePrefab, spawnPoint.position, transform.rotation);
    }
}
