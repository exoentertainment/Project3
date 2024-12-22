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

    public void LoadControlsWindow()
    {
        GameObject window = (GameObject)Instantiate(Resources.Load("Controls Window"));
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
