using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] bool resetSpeedsOnSceneChange = true;
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
        SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadedSceneMode)
        {
            if (resetSpeedsOnSceneChange)
                ResetAudioSpeed();
        };
    }
    public void ResetAudioSpeed()
    {
        musicSource.pitch = 1;
        sfxSource.pitch = 1;
    }
    public void SetMusicSpeed(float newSpeed){
        musicSource.pitch = newSpeed;
    }
    public void SetSfxSpeed(float newSpeed){
        sfxSource.pitch = newSpeed;
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if(s != null)
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if(s != null)
        {
            if (soundCooldowns.ContainsKey(name) && Time.time - soundCooldowns[name] < cooldownTime) return; // Avoids saturating
            soundCooldowns[name] = Time.time;
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void PlayManaSound(string name)
    {
        Sound s = Array.Find(manaSounds, x => x.name == name);

        if (s != null) manaSource.PlayOneShot(s.clip);
    }
    public void StopMusic()
    {
        if (musicSource.isPlaying) musicSource.Stop();
    }
    public void StopSFX()
    {
        sfxSource.Stop();
    }
    public void SetMusicVolume(float volume)
    {
        //musicSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }
    public void SetSFXVolume(float volume)
    {
      // sfxSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }
}
