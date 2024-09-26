using System;
using UnityEngine;

public class WeaponPlatformWindow : MonoBehaviour
{
    [Header("Prefabs")] 
    [SerializeField] private GameObject[] weaponPlatformsPrefabs;
    
    private GameObject weaponPlatform;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    enum PlatformType
    {
        Basic = 0
    }

    public void AssignWeaponPlatformSlot(GameObject slot)
    {
        weaponPlatform = slot;
    }

    public void PlaceWeaponPlatform()
    {
        weaponPlatform.GetComponent<WeaponPlatformSlot>().PlaceWeaponPlatform(weaponPlatformsPrefabs[(int)PlatformType.Basic]);
        
        CloseWindow();
    }
    
    public void CloseWindow()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
