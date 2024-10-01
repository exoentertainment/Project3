using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region -- Serialized Fields --
    
    [Header("Scriptable Object")] 
    [SerializeField] private EnemySpawnerScriptableObject spawnerSO;

    [Header("Component")] 
    [SerializeField] private Transform[] spawnPoint;
    
    #endregion
    
    float lastSpawnTime;

    private void Start()
    {
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if(Time.timeScale == 1)
            if ((Time.time - lastSpawnTime) > spawnerSO.spawnTime)
            {
                Debug.Log("spawn enemy");
                
                lastSpawnTime = Time.time;
            }
    }
}
