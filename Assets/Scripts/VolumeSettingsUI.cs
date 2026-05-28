using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsUI : MonoBehaviour
{
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SoundFXVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
    }
}