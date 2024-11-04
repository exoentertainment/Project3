using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Enemies/Spawner ScriptableObject")]
public class EnemySpawnerScriptableObject : ScriptableObject
{
    public int numSpawns;
    public float timeBetweenSpawns;
    public GameObject[] enemyShipsPrefabs;
}
