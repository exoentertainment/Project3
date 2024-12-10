using MoreMountains.Tools;
using UnityEngine;

public class AsteroidBeltObject : MonoBehaviour
{
    #region -- Serialized Fields
    
    [Header("Scriptable Objects")]
    [SerializeField] AsteroidSO asteroidSO;
    
    [Header("Variables")]
    [SerializeField] int minYOffset;
    [SerializeField] int maxYOffset;
    [SerializeField] private int minOrbitSpeed;
    [SerializeField] private int maxOrbitSpeed;
    
    [Header("Components")]
    [SerializeField] Rigidbody[] rb;
    [SerializeField] MMAutoRotate autoRotate;

    #endregion
    
    void Start()
    {
        SetMaterial();
        SetYOffset();
        SetOrbitSpeed();
        Spin();
    }

    void SetYOffset()
    {
        autoRotate.OrbitCenterOffset.y = Random.Range(minYOffset, maxYOffset);
    }

    void SetOrbitSpeed()
    {
        autoRotate.OrbitRotationSpeed = Random.Range(minOrbitSpeed, maxOrbitSpeed);
    }
    
    void SetMaterial()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        int materialNum = Random.Range(0, asteroidSO.materials.Length);

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = asteroidSO.materials[materialNum];
        }
    }
    
    void Spin()
    {
        foreach (Rigidbody rigidBody in rb)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rigidBody.AddTorque(randomDirection * 10);
        }
    }
}
