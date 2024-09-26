using System;
using UnityEngine;

public class WeaponPlatformWindow : MonoBehaviour
{
    [Header("Prefabs")] 
    [SerializeField] private GameObject[] weaponPlatformsPrefabs;
    
    private GameObject weaponPlatform;

    enum PlatformType
    {
        Basic = 0
    }

    //Take the passed weapon platform slot and store it if player builds a platform
    public void AssignWeaponPlatformSlot(GameObject slot)
    {
        weaponPlatform = slot;
    }

    //Pass the prefab of the basic platform to the platform slot for instantiation
    public void PlaceWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        weaponPlatform.GetComponent<WeaponPlatformSlot>().PlaceWeaponPlatform(weaponPlatformsPrefabs[(int)PlatformType.Basic]);
        
        CloseWindow();
    }
    
    //Resume the game and disable the platform selection window
    public void CloseWindow()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
