using UnityEngine;

[CreateAssetMenu (menuName = "Enemies/Spawner ScriptableObject")]
public class EnemySpawnerScriptableObject : ScriptableObject
{
    public int spawnTime;
    public int enemiesPerSpawn;
    public GameObject[] enemyShipsPrefabs;
}
