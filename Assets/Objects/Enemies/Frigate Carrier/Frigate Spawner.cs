using UnityEngine;

public class FrigateSpawner : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] private float spawnTime;
    
    [Header("Prefabs")]
    [SerializeField] GameObject[] frigatePrefabs;

    #endregion
    
    float lastSpawnTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnFrigate();
    }

    void SpawnFrigate()
    {
        if ((Time.time - lastSpawnTime) > spawnTime)
        {
            lastSpawnTime = Time.time;
            Instantiate(frigatePrefabs[Random.Range(0, frigatePrefabs.Length)], transform.position, transform.rotation);
        }
    }
}
