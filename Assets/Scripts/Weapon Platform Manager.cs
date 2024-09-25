using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WeaponPlatformManager : MonoBehaviour
{
    [Header("Serialized Variables")] 
    [SerializeField] private LayerMask weaponLayerMask;
    
    public static WeaponPlatformManager instance;
    
    [HideInInspector]
    public UnityEvent enablePlatformMeshesEvent;
    [HideInInspector]
    public UnityEvent disablePlatformMeshesEvent;
    
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

    public void ShowWeaponPlatforms(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            enablePlatformMeshesEvent?.Invoke();
        }
        else if (context.canceled)
        {
            disablePlatformMeshesEvent?.Invoke();
        }
    }

    public void SelectWeaponPlatform(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, weaponLayerMask))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.TryGetComponent<iInteractable>(out iInteractable slot)) 
                    {
                        slot.PrimaryInteract();
                    }
                    else
                    {

                    }
                }
            }
        }
    }
}
