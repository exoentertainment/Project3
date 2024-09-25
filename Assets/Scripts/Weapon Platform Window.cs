using UnityEngine;

public class WeaponPlatformWindow : MonoBehaviour
{
    private GameObject weaponPlatform;

    public void AssignWeaponPlatformSlot(GameObject slot)
    {
        weaponPlatform = slot;
        Debug.Log("done");
    }
}
