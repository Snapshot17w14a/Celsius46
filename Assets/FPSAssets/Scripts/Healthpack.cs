using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthpack : PickUpItem
{
    [SerializeField]
    private int restoredHealthAmount = 20;

    private new void ApplyEffect()
    {
        base.ApplyEffect();
    }

    public int GetRestoredHealthAmount()
    {
        return restoredHealthAmount;
    }
}
