using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammopack : PickUpItem
{
    [SerializeField]
    private int restoredAmmoAmount = 12;

    private new void ApplyEffect()
    {
        base.ApplyEffect();
    }

    public int GetRestoredAmmoAmount()
    {
        return restoredAmmoAmount;
    }
}
