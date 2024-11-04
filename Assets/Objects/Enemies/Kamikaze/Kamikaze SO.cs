using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeSO", menuName = "Enemies/Kamikaze")]
public class KamikazeSO : ScriptableObject
{
    public int maxHealth;
    public int moveSpeed;
    public int kamikazeSpeed;
    public int turnRate;
    public float coastDelay;
    public int accelerationDistance;
    public int damage;
    public int resourceReward;
    public LayerMask targetLayer;
    public GameObject shipPrefab;
    public GameObject explosionPrefab;
}
