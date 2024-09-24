using System;
using UnityEngine;

public class WeaponPlatformSlot : MonoBehaviour, iInteractable
{
    private void Start()
    {
        PlanetControlManager.disablePlatformMeshesEvent.AddListener(DisableMeshRenderer);
    }

    void DisableMeshRenderer()
    {
        Debug.Log("Disabling mesh renderer");
        GetComponent<MeshRenderer>().enabled = false;
    }
    
    public void PrimaryInteract()
    {
        Debug.Log("Primary interact");
    }

    public void SecondaryInteract()
    {
        
    }
}
