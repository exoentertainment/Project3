using UnityEngine;

public class ControlsWindow : MonoBehaviour
{
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }
}
