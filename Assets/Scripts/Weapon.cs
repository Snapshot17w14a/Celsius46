using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;

    private void Start()
    {
        Debug.Log(weaponData.weaponName);
    }
}
