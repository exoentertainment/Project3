using System;
using TMPro;
using UnityEngine;

public class WeaponPlatformWindow : MonoBehaviour
{
    [Header("Scriptable Objects")] 
    [SerializeField] private PlatformScriptableObject[] weaponPlatformsSO;

    [Header("Components")] 
    [SerializeField] private TMP_Text basicGunText;
    [SerializeField] private TMP_Text dualGunText;
    [SerializeField] private TMP_Text lightMissileText;
    [SerializeField] private TMP_Text cruiseMissileText;
    
    private GameObject weaponPlatform;

    enum PlatformType
    {
        Basic = 0,
        DualGun = 1,
        LightMissile = 2,
        CruiseMissile = 3
    }

    private void Start()
    {
        SetButtonTexts();
    }

    void SetButtonTexts()
    {
        basicGunText.text = "Basic Gun: " + weaponPlatformsSO[(int)PlatformType.Basic].resourceCost + " resources";
        dualGunText.text = "Dual Gun: " + weaponPlatformsSO[(int)PlatformType.DualGun].resourceCost + " resources";
        lightMissileText.text = "Light Missile: " + weaponPlatformsSO[(int)PlatformType.LightMissile].resourceCost + " resources";
        cruiseMissileText.text = "Cruise Missile: " + weaponPlatformsSO[(int)PlatformType.CruiseMissile].resourceCost + " resources";
    }
    
    //Take the passed weapon platform slot and store it if player builds a platform
    public void AssignWeaponPlatformSlot(GameObject slot)
    {
        weaponPlatform = slot;
    }

    //Pass the prefab of the basic platform to the platform slot for instantiation
    public void PlaceBasicWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.Basic].resourceCost))
        {
            if(weaponPlatform.transform.childCount > 0)
                CheckExistingPlatform(weaponPlatformsSO[(int)PlatformType.Basic].resourceCost/2);
            
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.Basic].platformPrefab);
            
            AudioManager.instance.PlayUISelect();
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.Basic].resourceCost);
            
            CloseWindow(false);
        }
        else
        {
            AudioManager.instance.PlayUIError();
        }

    }
    
    public void PlaceDualGunWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.DualGun].resourceCost))
        {
            if(weaponPlatform.transform.childCount > 0)
                CheckExistingPlatform(weaponPlatformsSO[(int)PlatformType.DualGun].resourceCost/2);
            
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.DualGun].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.DualGun].resourceCost);
            
            CloseWindow(false);
        }
        else
        {
            AudioManager.instance.PlayUIError();
        }
    }
    
    public void PlaceLightMissileWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.LightMissile].resourceCost))
        {
            if(weaponPlatform.transform.childCount > 0)
                CheckExistingPlatform(weaponPlatformsSO[(int)PlatformType.LightMissile].resourceCost/2);
            
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.LightMissile].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.LightMissile].resourceCost);
            
            CloseWindow(false);
        }
        else
        {
            AudioManager.instance.PlayUIError();
        }
    }
    
    public void PlaceCruiseMissileWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.CruiseMissile].resourceCost))
        {
            if(weaponPlatform.transform.childCount > 0)
                CheckExistingPlatform(weaponPlatformsSO[(int)PlatformType.CruiseMissile].resourceCost/2);
            
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.CruiseMissile].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.CruiseMissile].resourceCost);
            
            CloseWindow(false);
        }
        else
        {
            AudioManager.instance.PlayUIError();
        }
    }

    void CheckExistingPlatform(int value)
    {
        if(weaponPlatform.transform.childCount > 0)
            ResourceManager.instance.IncreaseResources(value);
    }
    
    //Resume the game and disable the platform selection window
    public void CloseWindow(bool playSound)
    {
        if(playSound)
            AudioManager.instance.PlayUIClose();
        
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
