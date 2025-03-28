using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource effectSource;
    
    // Sound effect files.
    public AudioClip bounceSFX;
    public AudioClip dashSFX;
    public AudioClip jumpSFX;

    // Music files.
    public AudioClip level1Music;

    public void PlayOneShot(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
