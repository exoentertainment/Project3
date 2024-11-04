using UnityEngine;

[CreateAssetMenu (menuName = "Enemies/Basic Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public int maxHealth;
    public int moveSpeed;
    public int turnRate;
    public int resourceReward;
    public float standOffDistance;
    public float coastDelay;
    public LayerMask targetLayer;
    public GameObject shipPrefab;
    public GameObject explosionPrefab;
}
