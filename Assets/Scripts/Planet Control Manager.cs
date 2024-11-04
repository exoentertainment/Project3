using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlanetControlManager : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] LayerMask cameraLayerMask;

    #endregion

    public static PlanetControlManager instance;
    
    [HideInInspector]
    public UnityEvent disablePlatformMeshesEvent;

    private GameObject currentOrbitalSlot;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CheckPlanetInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, cameraLayerMask))
            {
                if (hit.collider != null)
                {
                   if (hit.collider.gameObject.TryGetComponent<iInteractable>(out iInteractable slot)) 
                   {
                       currentOrbitalSlot = hit.collider.gameObject;
                       //slot.PlaceBasicWeaponPlatform();
                        slot.Interact();
                   }
                   else
                   {
                       disablePlatformMeshesEvent?.Invoke();
                   }
                }
            }
        }
    }
}
