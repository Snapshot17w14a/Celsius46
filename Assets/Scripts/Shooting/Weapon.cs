using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject muzzleFlashPoint;
    [SerializeField] private GameObject bulletTrace;

    private readonly List<LineRenderer> traceLines = new();

    public WeaponData.WeaponType WeaponType => weaponData.weaponType;
    public float FireRate => weaponData.rateOfFire;
    public int MaxAmmo => weaponData.maxAmmo;

    protected int currentAmmo;
    protected int currentAmmoInGun = 100;

    protected static PlayerController player;

    private void Start()
    {
        Debug.Log(weaponData.weaponName);
        if (player == null) player = FindAnyObjectByType<PlayerController>();
    }

    private void Update()
    {
        if(traceLines.Count != 0) UpdateTraces();
    }

    private void UpdateTraces()
    {
        foreach(LineRenderer line in traceLines)
        {
            
        }
    }

    protected void PlayMuzzleFlash()
    {
        Destroy(Instantiate(muzzleFlash, muzzleFlashPoint.transform.position, muzzleFlashPoint.transform.rotation, muzzleFlashPoint.transform), 0.1f);
    }

    protected void DrawTrace(Vector3 target)
    {
        var line = Instantiate(bulletTrace, Vector3.zero, Quaternion.identity);
        Destroy(line, 0.1f);
        line.GetComponent<LineRenderer>().SetPositions(new[] { muzzleFlashPoint.transform.position, target });
    }

    protected Collider RayCastShot(float distance, out Vector3 point)
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * distance, Color.red, 1);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, distance, LayerMask.GetMask("Enemy")))
        {
            GameEvents.RaiseOnPlayerHitShot(hit);
            point = hit.point;
            return hit.collider;
        }
        point = Vector3.zero;
        return null;
    }
}