using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenButtonsManager : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
