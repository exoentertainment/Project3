using System;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPlatformSlot : MonoBehaviour, iInteractable
{
    //This holds the installed weapon platform
    private GameObject weaponPlatform;

    [SerializeField] GameObject weaponPlatformWindow;
    
    private void Start()
    {
        WeaponPlatformManager.instance.disablePlatformMeshesEvent.AddListener(DisableMeshRenderer);
        WeaponPlatformManager.instance.enablePlatformMeshesEvent.AddListener(EnableMeshRenderer);
        
        Debug.Log(weaponPlatformWindow.name);
    }

    void DisableMeshRenderer()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    void EnableMeshRenderer()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }
    
    public void PlaceWeaponPlatform()
    {
    }
    
    public void PrimaryInteract()
    {
        weaponPlatformWindow.SetActive(true);
        weaponPlatformWindow.GetComponent<WeaponPlatformWindow>().AssignWeaponPlatformSlot(weaponPlatform);
    }

    public void SecondaryInteract()
    {
        
    }
}
