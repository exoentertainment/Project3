using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void OpenSettings()
    {
        GameObject audioSettings = (GameObject)Instantiate(Resources.Load("Audio Settings"));
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
