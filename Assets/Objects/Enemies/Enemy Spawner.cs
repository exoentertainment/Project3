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
                for (int x = 0; x < spawnerSO.enemiesPerSpawn; x++)
                {
                    GameObject enemy = Instantiate(spawnerSO.enemyShipsPrefabs[0], spawnPoint[x].position,
                        Quaternion.identity);
                    enemy.transform.SetParent(transform.parent);
                }

                lastSpawnTime = Time.time;
            }
    }
}
