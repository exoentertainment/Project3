using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScreenExit : MonoBehaviour
{
    public void ContinueToGame()
    {
        SceneManager.LoadScene(1);
    }
}
