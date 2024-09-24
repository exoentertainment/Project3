using UnityEngine;
using UnityEngine.Events;

public class WeaponPlatformManager : MonoBehaviour, iInteractable
{
    [Header("Components")]
    [SerializeField] Transform[] platformTransforms;
    
    [Header("Events")]
    [SerializeField] UnityEvent onInteract;
    
    //Display platform slots and bring up platform selection screen
    public void PrimaryInteract()
    {
        onInteract?.Invoke();
    }

    public void SecondaryInteract()
    {
        
    }

    public void AddPlatform(GameObject platform)
    {
        
    }
}
