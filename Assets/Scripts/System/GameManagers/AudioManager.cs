using UnityEngine;

using System.Collections.Generic;

namespace MagmaLabs.Audio{
    [RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public static AudioManager instance;

    public AudioClip[] reusableSounds;
    public AudioClip[] musicTracks;
    public List<GameObject> activeMusicPlayers = new List<GameObject>();
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

    public void PlaySoundWithRandomPitchShift(string soundName, float volume, float range = 0.2f)
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

    public void PlayMusic(string trackName, float volume, bool loop = true)
    {
        foreach (AudioClip track in musicTracks)
        {
            if (track.name == trackName)
            {
                GameObject musicPlayer = new GameObject("MusicPlayer_" + trackName);
                AudioSource musicSource = musicPlayer.AddComponent<AudioSource>();
                musicSource.clip = track;
                musicSource.volume = volume;
                musicSource.loop = loop;
                musicSource.Play();
                activeMusicPlayers.Add(musicPlayer);
                DontDestroyOnLoad(musicPlayer);
                return;
            }
        }
        Debug.LogError("Music track not found: " + trackName);
    }

    public void StopMusic(string trackName)
    {
        for (int i = activeMusicPlayers.Count - 1; i >= 0; i--)
        {
            AudioSource musicSource = activeMusicPlayers[i].GetComponent<AudioSource>();
            if (musicSource.clip.name == trackName)
            {
                Destroy(activeMusicPlayers[i]);
                activeMusicPlayers.RemoveAt(i);
            }
        }
    }    
}
    
}
