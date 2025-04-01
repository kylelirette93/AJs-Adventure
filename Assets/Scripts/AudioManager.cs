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
    public AudioClip eatSFX;
    public AudioClip deathSFX;

    // Music files.
    public AudioClip mainMenuMusic;
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
