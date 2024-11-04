using System;
using MoreMountains.Tools;
using UnityEngine;

public class OrbitSpeedSetter : MonoBehaviour
{
    #region -- Serialized Fields --
    
    [Header("Components")]
    [SerializeField] MMAutoRotate autoRotate;

    [Header("Settings")] 
    [SerializeField] private float distanceSpeedRatio;

    #endregion

    private void Start()
    {
        SetOrbitSpeed();
    }

    void SetOrbitSpeed()
    {
        autoRotate.OrbitRotationSpeed = autoRotate.OrbitRadius / distanceSpeedRatio;
    }
}
