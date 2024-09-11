using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private Weapon activeWeapon;
    [SerializeField] private Weapon[] allWeapons;

    private Vector3 startingOffset;

    private IShootable activeWeaponInterface;
    private float lastFireTime = 0;

    private void Start()
    {
        activeWeaponInterface = GetActiveWeaponInterface();
        startingOffset = transform.position;
    }

    private void Update()
    {
        ApplyCameraRotation();
        CheckForShooting();
    }

    private void CheckForShooting()
    {
        // Check if the player is trying to shoot and use the correct method based on the weapon type,
        // also check if the weapon is ready to shoot again, and keep the last time the weapon was fired.
        // If the weapon is unsuccessful in shooting, keep the last time the weapon was fired.
        switch (activeWeapon.WeaponType)
        {
            case WeaponData.WeaponType.Automatic:
                Debug.Log("Automatic");
                if (Input.GetMouseButton(0) && lastFireTime + activeWeapon.FireRate < Time.time * 1000) lastFireTime = activeWeaponInterface.Shoot() ? Time.time * 1000 : lastFireTime;
                break;
            case WeaponData.WeaponType.SingleFire:
            case WeaponData.WeaponType.ProjectileLauncher:
                if (Input.GetMouseButtonDown(0) && lastFireTime + activeWeapon.FireRate < Time.time * 1000) lastFireTime = activeWeaponInterface.Shoot() ? Time.time * 1000 : lastFireTime;
                break;
        }
    }

    private void ApplyCameraRotation()
    {
        //transform.position = Camera.main.transform.rotation * startingOffset;
    }

    public void SwitchWeapon(int weaponIndex)
    {
        // Destroy the current weapon and instantiate the new weapon, then set the active weapon and its interface.
        Destroy(activeWeapon.gameObject);
        Instantiate(allWeapons[weaponIndex], transform);
        activeWeapon = allWeapons[weaponIndex];
        activeWeaponInterface = GetActiveWeaponInterface();
    }

    private IShootable GetActiveWeaponInterface() => activeWeapon.GetComponent<IShootable>();
}
