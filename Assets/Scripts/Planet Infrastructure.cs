using UnityEngine;
using Time = UnityEngine.Time;

public class PlanetInfrastructure : MonoBehaviour, iInteractable
{
    [Header("Variables")] 
    [SerializeField] private int maxInfrastructureLevels;
    [SerializeField] private float resourcesToLevelUp;

    [Header("Prefabs")] 
    [SerializeField] private GameObject infrastructureWindow;


    //Load the infrastructure info window
    public void PrimaryInteract()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            infrastructureWindow.SetActive(true);
        }
    }

    public void SecondaryInteract()
    {
        
    }
}
