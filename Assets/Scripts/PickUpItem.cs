using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PickUpItem : MonoBehaviour, IPickable, ISoundEmitting
{
    [SerializeField]
    private AudioClip audioClip;

    public void ApplyEffect()
    {
        GameEvents.RaiseOnPlayerPickUpItem(this);
        PlayObjectSpecificSound();
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
