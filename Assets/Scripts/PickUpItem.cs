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
        if (collision.collider.tag == "Player")
        {
            ApplyEffect();
            //Destroy(gameObject);
        }
    }

    public void PlayObjectSpecificSound()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        print(audioSource.isPlaying);
        print("tried playing sound...");
    }
}
