using UnityEngine;

[CreateAssetMenu(fileName = "New Platform Health SO", menuName = "Platform Health SO")]
public class PlatformHealthSO : ScriptableObject
{
    public int maxHealth;
    public GameObject explosionPrefab;
}
