using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlanetControlManager : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] LayerMask cameraLayerMask;

    #endregion

    public static UnityEvent disablePlatformMeshesEvent;
    
    public void CheckPlanetInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, cameraLayerMask))
            {
                if (hit.collider != null)
                {
                   if (hit.collider.gameObject.TryGetComponent<iInteractable>(out iInteractable interactable)) 
                   {
                        interactable.PrimaryInteract();
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
