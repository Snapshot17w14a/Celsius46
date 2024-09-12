using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerShooting : MonoBehaviour
{

    [SerializeField]
    private Transform rayStartPoint;
    private float maxDistance = Mathf.Infinity, fireRate = 0.2f; //fireRate = 0 => shoot once
                                                          //else interval between shots is (fireRate) seconds.
    [SerializeField]
    private LayerMask layerMask;
    private Coroutine currentRunningRoutine;
    private bool routineActive;
    private WeaponScriptableObject equippedWeapon;
    public WeaponScriptableObject EquippedWeapon { get => equippedWeapon; set => SetWeapon(); }

    private void Start()
    {
        equippedWeapon = EquippedWeapon;
        SetWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            if (!routineActive)
                currentRunningRoutine = StartCoroutine(FireAtRate());
    }

    private IEnumerator FireAtRate()
    {
        routineActive = true;
        Shoot();
        yield return new WaitForSeconds(fireRate);
        if (Input.GetKey(KeyCode.Return))
        {
            if (fireRate.CompareTo(0f) != 0)
            {
                StopCoroutine(currentRunningRoutine);
                currentRunningRoutine = StartCoroutine(FireAtRate());
            }
        }
        else
            routineActive = false;
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayStartPoint.position, rayStartPoint.forward, out hit, maxDistance, layerMask))
        {
            Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), hit.point, Quaternion.identity);
            GameEvents.RaiseOnPlayerHitShot(hit);
        }
        GameEvents.RaiseOnPlayerShoot();
    }

    public void SetWeapon()
    {
        //equippedWeapon = EquippedWeapon;
        //fireRate = equippedWeapon.fireRate;
        //.....
    }

}
