using System;
using UnityEngine;

public class StationRingRotation : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool rotateX;
    [SerializeField] private bool rotateY;
    [SerializeField] private bool rotateZ;

    #endregion

    private void Update()
    {
        RotateRing();
    }

    void RotateRing()
    {
        if(Time.timeScale == 1)
            if (rotateX)
            {
                transform.rotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime, 0, 0);
            }
            else if(rotateY)
            {
                transform.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
            }
            else if (rotateZ)
            {
                transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
            }
    }
}
