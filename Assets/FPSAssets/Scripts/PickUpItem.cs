using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour, IPickable
{
    public void ApplyEffect()
    {
        GameEvents.RaiseOnPlayerPickUpItem(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CompareTag("Player"))
        {
            ApplyEffect();
            Destroy(gameObject);
        }
    }
}
