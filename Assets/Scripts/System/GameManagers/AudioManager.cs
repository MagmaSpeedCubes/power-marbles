using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public static AudioManager instance;

    public AudioClip[] reusableSounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("Multiple instances detected. Destroying duplicate");
            Destroy(this);
        }
    }

    public void PlaySound(AudioClip sound, float volume)
    {
        audioSource.PlayOneShot(sound, volume);
    }

    public void PlaySound(string soundName, float volume)
    {
        foreach (AudioClip sound in reusableSounds)
        {
            if (sound.name == soundName)
            {
                audioSource.PlayOneShot(sound, volume);
                return;
            }
        }
        Debug.LogError("Sound not found: " + soundName);
    }
    


    
}

