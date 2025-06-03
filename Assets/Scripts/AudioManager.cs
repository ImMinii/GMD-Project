using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource SFXSound;
    
    public AudioClip collection;
    public AudioClip powerUp;

    public void PlayCollecion()
    {
        SFXSound.PlayOneShot(collection);
    }

    public void PlayPowerUpCollect(AudioClip clip)
    {
        SFXSound.PlayOneShot(clip);
    }
    
}
