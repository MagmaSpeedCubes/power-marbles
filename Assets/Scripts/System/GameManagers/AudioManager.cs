using UnityEngine;

using System.Collections.Generic;
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public static AudioManager instance;

    public AudioClip[] reusableSounds;
    /*
    SOUNDS REQUIRED
    pop
    select
    begin
    marble-clink
    whoosh
    loss
    reward
    jackpot
    */

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Multiple instances detected. Destroying duplicate");
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip sound, float volume)
    {
        audioSource.PlayOneShot(sound, volume);
    }

    public void PlaySoundWithPitchShift(string soundName, float volume, float range = 0.2f)
    {
        foreach (AudioClip sound in reusableSounds)
        {
            if (sound.name == soundName)
            {

                audioSource.pitch = Random.value * range + (1-range/2);
                audioSource.PlayOneShot(sound, volume);
                return;
            }
        }
        Debug.LogError("Sound not found: " + soundName);
    }

    public void PlaySound(string soundName, float volume)
    {
        foreach (AudioClip sound in reusableSounds)
        {
            if (sound.name == soundName)
            {

                audioSource.pitch = 1;
                audioSource.PlayOneShot(sound, volume);
                return;
            }
        }
        Debug.LogError("Sound not found: " + soundName);
    }

    
    


    
}

