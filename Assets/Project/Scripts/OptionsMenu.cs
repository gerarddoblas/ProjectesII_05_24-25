using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle fullscreen;
    public Slider brightSlider;
    public Slider musicSlider,fxSoundSlider;
    public Button Return;
    public TMP_Dropdown langSelector;
        void Start()
        {
            Screen.fullScreen = (PlayerPrefs.GetInt("fullScreen", 1) == 1);
            fullscreen.isOn = Screen.fullScreen;
            fullscreen.onValueChanged.AddListener(delegate (bool r) {
                Screen.fullScreen = r;
                PlayerPrefs.SetInt("fullScreen", r ? 1 : 0);
                PlayerPrefs.Save();
            });

            Screen.brightness = PlayerPrefs.GetFloat("brightness", 1);
            brightSlider.value = Screen.brightness;
            brightSlider.onValueChanged.AddListener(delegate (float r) {
                Screen.brightness = r;
                PlayerPrefs.SetFloat("brightness", r);
                PlayerPrefs.Save();
            });

            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
            AudioManager.instance.musicSource.volume = musicSlider.value;
            musicSlider.onValueChanged.AddListener(delegate (float r) {
                AudioManager.instance.musicSource.volume = r;
                PlayerPrefs.SetFloat("MusicVolume", r);
                PlayerPrefs.Save();
            });

            fxSoundSlider.value = PlayerPrefs.GetFloat("FXVolume", 1);
            AudioManager.instance.sfxSource.volume = fxSoundSlider.value;
            fxSoundSlider.onValueChanged.AddListener(delegate (float r) {
                AudioManager.instance.sfxSource.volume = r;
                PlayerPrefs.SetFloat("FXVolume", r);
                PlayerPrefs.Save();
            });

            langSelector.onValueChanged.AddListener(delegate (int optionIndex) {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[optionIndex];
            });
        }
}
