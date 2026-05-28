using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager instance;

    [SerializeField] private AudioMixer masterMixer;

    private void Awake()
    {
        // singleton
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        LoadVolumeSettings();
    }

    // MASTER
    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20f);

        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    // SFX
    public void SetSoundFXVolume(float volume)
    {
        masterMixer.SetFloat("SoundFXVolume", Mathf.Log10(volume) * 20f);

        PlayerPrefs.SetFloat("SoundFXVolume", volume);
    }

    // MUSIC
    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);

        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void LoadVolumeSettings()
    {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        float soundFXVolume = PlayerPrefs.GetFloat("SoundFXVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);

        SetMasterVolume(masterVolume);
        SetSoundFXVolume(soundFXVolume);
        SetMusicVolume(musicVolume);
    }
}