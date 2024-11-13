using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPlatformSpawner : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Objects")]
    [SerializeField] PlatformScriptableObject[] platforms;
    
    [Header("Components")]
    [SerializeField] Transform[] platformSlots;

    #endregion

    private void Start()
    {
        PlacePlatforms();
    }

    void PlacePlatforms()
    {
        foreach (Transform slot in platformSlots)
        {
            Instantiate(platforms[Random.Range(0, platforms.Length)].platformPrefab, slot.position, Quaternion.identity, slot);
        }
    }
}
