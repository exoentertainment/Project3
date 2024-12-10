using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] private int minspawnFrequency;
    [SerializeField] private int maxspawnFrequency;
    
    [Header("Components")]
    [SerializeField] SphereCollider sphereCollider;
    
    [Header("Prefabs")]
    [SerializeField] GameObject[] asteroidPrefabs;

    #endregion

    private int spawnFrequency;
    float lastSpawnTime;
    
    private void Start()
    {
        spawnFrequency = Random.Range(minspawnFrequency, maxspawnFrequency);
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        SpawnAsteroid();
    }

    void SpawnAsteroid()
    {
        if (Time.time - lastSpawnTime > spawnFrequency)
        {
            spawnFrequency = Random.Range(minspawnFrequency, maxspawnFrequency);
            lastSpawnTime = Time.time;
            
            Vector3 spawnPos = transform.position + Random.onUnitSphere * (sphereCollider.radius * transform.lossyScale.x);
            Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], spawnPos, Quaternion.identity);
        }
    }
}
