using UnityEngine;

public class PlanetInfrastructureDisplay : MonoBehaviour
{
    public void CloseWindow()
    {
        Time.timeScale = 1;
        Debug.Log("Close Window");
        gameObject.SetActive(false);
    }
}
