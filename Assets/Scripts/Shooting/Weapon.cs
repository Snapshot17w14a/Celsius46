using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] private ParticleSystem muzzleFlash;

    public WeaponData.WeaponType WeaponType => weaponData.weaponType;
    public float FireRate => weaponData.rateOfFire;
    public int MaxAmmo => weaponData.maxAmmo;

    protected int currentAmmo;
    protected int currentAmmoInGun;

    protected static PlayerController player;

    private void Start()
    {
        Debug.Log(weaponData.weaponName);
        if (player == null) player = FindAnyObjectByType<PlayerController>();
    }

    protected void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    protected Collider RayCastShot(float distance)
    {
        RaycastHit hit;
        Debug.DrawRay(player.transform.position, Camera.main.transform.forward * distance, Color.red, 1);
        if (Physics.Raycast(player.transform.position, Camera.main.transform.forward, out hit, distance, LayerMask.GetMask("Enemy")))
        {
            //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), hit.point, Quaternion.identity);
            GameEvents.RaiseOnPlayerHitShot(hit);
            return hit.collider;
        }
        return null;
    }
}