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
        // weaponPlatform = new GameObject();
        WeaponPlatformManager.instance.disablePlatformMeshesEvent.AddListener(DisableMeshRenderer);
        WeaponPlatformManager.instance.enablePlatformMeshesEvent.AddListener(EnableMeshRenderer);
    }

    void DisableMeshRenderer()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    void EnableMeshRenderer()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }
    
    public void PlaceWeaponPlatform(GameObject weaponPlatformObject)
    {
        weaponPlatform = Instantiate(weaponPlatformObject, transform.position, Quaternion.identity);
        weaponPlatform.transform.SetParent(transform);
    }
    
    public void PrimaryInteract()
    {
        Time.timeScale = 0;
        weaponPlatformWindow.SetActive(true);
        weaponPlatformWindow.GetComponent<WeaponPlatformWindow>().AssignWeaponPlatformSlot(this.gameObject);
    }

    public void SecondaryInteract()
    {
        
    }
}
