using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public void OpenPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                GameObject window = (GameObject)Instantiate(Resources.Load("Pause Menu"));
            }
        }
    }
}
