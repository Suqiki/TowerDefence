using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void PlaySoundEffect(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign audioClip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get lenght
        float clipLenght = audioSource.clip.length;

        //destroy clip after is done
        Destroy(audioSource.gameObject, clipLenght);
    }
    
    public void PlayrandomSoundEffect(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int randomIndex = Random.Range(0, audioClip.Length);
        
        //spawn gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign audioClip
        audioSource.clip = audioClip[randomIndex];

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get lenght
        float clipLenght = audioSource.clip.length;

        //destroy clip after is done
        Destroy(audioSource.gameObject, clipLenght);
    }
}
