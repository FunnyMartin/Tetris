using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    //Plays music by name.
    public void PlayMusic(string name)
    {
        try
        {
            Sound s = Array.Find(musicSounds, x => x.name == name);
            musicSource.clip = s.clip;
            musicSource.Play();
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //Plays sfx by name.
    public void PlaySFX(string name)
    {
        try
        {
            Sound s = Array.Find(sfxSounds, x => x.name == name);
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //When loading into a new scene it checks for instance, if there isnt it creates it, if there is it destroys this new one.
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Plays music MainMenu when application starts.
    private void Start()
    {
        PlayMusic("MainMenu");
    }

    //Sets music volume by input, used in settings on slider.
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    //Sets sfx volume by input, used in settings on slider.
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
