using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public enum WeaponType
    {
        SingleFire,         //Single shot per click
        Automatic,          //Continuous fire
        ProjectileLauncher  //Launches a projectile
    }

    public string weaponName; //Name of the weapon that will be displayed in the UI

    public WeaponType weaponType;

    public float reloadTime; //Milliseconds to reload
    public float rateOfFire; //Milliseconds between shots
    public float damage;     //Units of damage per shot
    public float range;      //Units of range

    public int maxAmmo;      //Maximum ammo capacity
}
