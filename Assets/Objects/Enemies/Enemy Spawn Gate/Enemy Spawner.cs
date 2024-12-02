using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    #region -- Serialized Fields --
    
    [Header("Variables")]
    [SerializeField] float timeBetweenWaves;
    [SerializeField] private float lightGain;
    
    [FormerlySerializedAs("spawnerSO")]
    [Header("Scriptable Object")] 
    [SerializeField] private EnemySpawnerScriptableObject[] spawnWavesSO;
    
    [Header("Component")] 
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private Light spawnLight;
    
    #endregion
    
    int currentWave;
    float defaultLightIntensity;
    
    private void Start()
    {
        SpawnEnemy();
        defaultLightIntensity = spawnLight.intensity;
    }
    
    void SpawnEnemy()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        if (Time.timeScale == 1)
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            int numWaves = spawnWavesSO.Length;

            while (numWaves > 0)
            {
                for (int i = 0; i < spawnWavesSO[currentWave].numSpawns; i++)
                {
                    int randomSpawnPoint = Random.Range(0, spawnPoint.Length);
                    int enemySpawn = Random.Range(0, spawnWavesSO[currentWave].enemyShipsPrefabs.Length);
                    
                    GameObject enemy = Instantiate(
                            spawnWavesSO[currentWave].enemyShipsPrefabs[enemySpawn],
                            spawnPoint[randomSpawnPoint].position,
                            Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,
                                transform.rotation.z));

                       if (numWaves == 1 && i == (spawnWavesSO[currentWave].numSpawns - 1))
                       { 
                           GameManager.instance.AssignLastEnemy(enemy);
                       }
                       
                    StartCoroutine(IncreaseLightIntensityRoutine());
                    yield return new WaitForSeconds(spawnWavesSO[currentWave].timeBetweenSpawns);
                }

                numWaves--;
                currentWave++;
                
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
    }

    IEnumerator IncreaseLightIntensityRoutine()
    {
        float lightOnTime = 0;

        while(true)
        {
            lightOnTime += Time.deltaTime;
            
            spawnLight.intensity += lightGain * Time.deltaTime;

            yield return null;

            if (lightOnTime >= spawnWavesSO[currentWave].timeBetweenSpawns)
                break;
        }
        
        spawnLight.intensity = defaultLightIntensity;
    }
}
