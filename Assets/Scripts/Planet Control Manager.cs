using UnityEngine;
using UnityEngine.InputSystem;

public class PlanetControlManager : MonoBehaviour
{
    public void CheckPlanetInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.TryGetComponent<iInteractable>(out iInteractable interactable))
                    {
                        interactable.PrimaryInteract();
                    }
                }
            }
        }
    }
}
