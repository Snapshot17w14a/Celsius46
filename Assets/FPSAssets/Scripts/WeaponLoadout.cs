using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLoadout : MonoBehaviour
{
    [SerializeField]
    private WeaponScriptableObject[] weapons;
    private int weaponIndex;
    PlayerShooting playerShooting;

    private void Start()
    {
        playerShooting = GetComponent<PlayerShooting>();
        print(playerShooting.EquippedWeapon == null);
        playerShooting.EquippedWeapon = weapons[weaponIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EquipNextWeapon();
        }
    }

    private void EquipNextWeapon()
    {
        weaponIndex++;
        if (weaponIndex > weapons.Length - 1)
            weaponIndex = 0;
        playerShooting.EquippedWeapon = weapons[weaponIndex];
        print(playerShooting.EquippedWeapon.fireRate);
    }
}
