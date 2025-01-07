using System;
using UnityEngine;

public class KamikazeAttack : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")] 
    [SerializeField] private KamikazeSO shipSO;

    #endregion

    private void OnCollisionEnter(Collision other)
    {
        if (1 << other.gameObject.layer == shipSO.targetLayer)
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hit))
            {
                hit.TakeDamage(shipSO.damage);
                Instantiate(shipSO.explosionPrefab, other.contacts[0].point, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
