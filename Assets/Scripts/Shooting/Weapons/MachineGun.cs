using UnityEngine;

public class MachineGun : Weapon, IShootable
{
    new int currentAmmoInGun = 50;

    public bool Shoot()
    {
        Debug.Log("Machine gun shooting");
        if (currentAmmoInGun == 0) return false;
        var target = RayCastShot(weaponData.range);
        if (target != null) target.GetComponent<IDamagable>().TakeDamage(weaponData.damage);
        PlayMuzzleFlash();
        return true;
    }

    public void Reload()
    {

    }
}
