using System;
using UnityEngine;

public class SpawnLight : MonoBehaviour
{
    #region -- Serialized Fields --
    
    [Header("Components")]
    [SerializeField] private Light light;
    
    [Header("Variables")]
    [SerializeField] private float lightIntensityGain;
    
    #endregion

    private void Update()
    {
        
    }
}
