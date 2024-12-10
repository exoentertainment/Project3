using UnityEngine;

[CreateAssetMenu (menuName = "Objects/Asteroid")]
public class AsteroidSO : ScriptableObject
{
    public float moveSpeed;
    public float health;
    public int damage;
    public int lifespan;
    public LayerMask targetLayers;
    public Material[] materials;
    public GameObject explosionPrefab;
}
