using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] musicSounds, sfxSounds, manaSounds;
    public AudioSource musicSource, sfxSource, manaSource;

    private Dictionary<string, float> soundCooldowns = new Dictionary<string, float>();
    private float cooldownTime = 0.15f; 
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


    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null) 
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            if (soundCooldowns.ContainsKey(name) && Time.time - soundCooldowns[name] < cooldownTime)
            {
                return; // Evita la saturación
            }
            soundCooldowns[name] = Time.time;
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void PlayManaSound(string name)
    {
        Sound s = Array.Find(manaSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            manaSource.PlayOneShot(s.clip);
        }
    }
    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
    public void StopSFX()
    {
        sfxSource.Stop();
    }
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }
    public void SetSFXVolume(float volume)
    {
       sfxSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }
}
