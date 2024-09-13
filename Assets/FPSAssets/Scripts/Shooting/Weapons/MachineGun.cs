using UnityEngine;

public class MachineGun : Weapon, IShootable
{
    public bool Shoot()
    {
        if (currentAmmoInGun == 0) return false;
        Collider target = RayCastShot(weaponData.range, out Vector3 hitPoint);
        if (target != null)
        {
            target.GetComponent<IDamagable>().TakeDamage(weaponData.damage);
            DrawTrace(hitPoint);
        }
        else DrawTrace(Camera.main.transform.position + Camera.main.transform.forward * weaponData.range);
        PlayMuzzleFlash();
        return true;
    }

    public void Reload()
    {

    }
}
