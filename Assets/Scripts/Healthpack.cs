using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthpack : PickUpItem
{
    public readonly int restoredHealthAmount = 20;

    private new void ApplyEffect()
    {
        base.ApplyEffect();
    }
}
