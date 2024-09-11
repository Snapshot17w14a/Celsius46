using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    enum WeaopnType
    {
        SingleFire,
        Automatic,
        ProjectileLauncher
    }

    public string weaponName;
}
