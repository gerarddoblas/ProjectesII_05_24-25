using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle fullscreen;
    public Slider brightSlider;
    public Slider musicSlider,fxSoundSlider;
    public Button Return;
    void Start()
    {
        fullscreen.onValueChanged.AddListener(delegate (bool r) { 
            Screen.fullScreen = r;
        });
        brightSlider.onValueChanged.AddListener(delegate (float r) {
            Screen.brightness = r;
        });
        musicSlider.onValueChanged.AddListener(delegate (float r) { 
            AudioManager.instance.manaSource.volume = r;
        });
        fxSoundSlider.onValueChanged.AddListener(delegate (float r) {
            AudioManager.instance.sfxSource.volume = r;
            AudioManager.instance.manaSource.volume = r;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
