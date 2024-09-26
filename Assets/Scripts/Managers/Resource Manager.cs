using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private int currentResources;
    
    public static ResourceManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Increase the current amount of resources by the passed parameter
    public void IncreaseResources(int value)
    {
        currentResources += value;
    }

    public void DecreaseResources(int value)
    {
        currentResources -= value;
    }
    
    //Check if the current amount of resources is at least the same as the passed parameter
    public bool CheckForResources(int value)
    {
        if(currentResources >= value)
            return true;
        else
            return false;
    }
}
