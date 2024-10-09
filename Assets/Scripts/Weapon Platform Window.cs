using System;
using UnityEngine;

public class WeaponPlatformWindow : MonoBehaviour
{
    [Header("Scriptable Objects")] 
    [SerializeField] private PlatformScriptableObject[] weaponPlatformsSO;
    
    private GameObject weaponPlatform;

    enum PlatformType
    {
        Basic = 0,
        DualGun = 1,
        LightMissile = 2
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
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.Basic].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.Basic].resourceCost);
        }
        else
        {
            //play sound
        }

        CloseWindow();
    }
    
    public void PlaceDualGunWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.DualGun].resourceCost))
        {
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.DualGun].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.DualGun].resourceCost);
        }
        else
        {
            //play sound
        }

        CloseWindow();
    }
    
    public void PlaceLightMissileWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.LightMissile].resourceCost))
        {
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.LightMissile].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.LightMissile].resourceCost);
        }
        else
        {
            //play sound
        }

        CloseWindow();
    }
    
    //Resume the game and disable the platform selection window
    public void CloseWindow()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
