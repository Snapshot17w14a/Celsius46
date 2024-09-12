using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour, IPickable
{
    public void ApplyEffect()
    {
        GameEvents.RaiseOnPlayerPickUpItem(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ApplyEffect();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            ApplyEffect();
            Destroy(gameObject);
        }
    }
}
