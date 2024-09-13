using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
[RequireComponent(typeof(AudioSource))]
public class PickUpItem : MonoBehaviour, IPickable, ISoundEmitting
{
    [SerializeField]
    private AudioClip audioClip;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            PlayObjectSpecificSound();
    }

    public void ApplyEffect()
    {
        GameEvents.RaiseOnPlayerPickUpItem(this);
        PlayObjectSpecificSound();
=======
public class PickUpItem : MonoBehaviour, IPickable
{
    public void ApplyEffect()
    {
        GameEvents.RaiseOnPlayerPickUpItem(this);
>>>>>>> origin/developement
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CompareTag("Player"))
        {
            ApplyEffect();
            Destroy(gameObject);
        }
    }
<<<<<<< HEAD

    public void PlayObjectSpecificSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        print(audioSource.isPlaying);
        print("tried playing audio...");
    }
=======
>>>>>>> origin/developement
}
