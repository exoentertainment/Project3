using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Clips", menuName = "Audio SO/Enemy Clips")]
public class EnemyClipsSO : ScriptableObject
{
    public AudioClipSO basicTurret;
    public AudioClipSO lightMissileTurret;
    public AudioClipSO rotaryTurret;
    public AudioClipSO smallExplosion;

    #region -- Boss SFX --

    public AudioClipSO boss1SpecialWeapon;
    public AudioClipSO boss1SpecialWeaponCharging;

    #endregion
}
