using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{ 
    /// <summary>Call to attempt a shot with weapon</summary>
    /// <returns>Whether the shoot was successful (shot can be unsuccessful when there is no ammo in the weapon)</returns>
    bool Shoot();
    void Reload();
}
