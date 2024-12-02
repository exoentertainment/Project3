using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClearedButton : MonoBehaviour
{
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
