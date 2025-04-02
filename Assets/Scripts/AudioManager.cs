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
    public AudioClip popSFX;

    // Music files.
    public AudioClip mainMenuMusic;
    public AudioClip level1Music;
    public bool IsPlaying { get { return isPlaying; } }
    bool isPlaying = false;

    public void PlayOneShot(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        isPlaying = true;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void StopMusic()
    {
        isPlaying = false;
        musicSource.Stop();
    }
}
