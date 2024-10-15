using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    #region --Clips--

    [Header("Audio Scriptable Objects")]
    [SerializeField] PlatformClipsSO platformClips;


    #endregion

    AudioSource musicAudioSource;

    public static AudioManager instance;

    private int levelOffset = 2;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        musicAudioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySound(AudioClipSO soundSO)
    {
        GameObject soundObject = new GameObject("Audio Clip Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = soundSO.clip;
        audioSource.loop = soundSO.loop;
        audioSource.volume = soundSO.volume;
        audioSource.pitch = soundSO.pitch;
        audioSource.outputAudioMixerGroup = soundSO.audioMixer;

        audioSource.Play();
        Destroy(soundObject, soundSO.clip.length);
    }

    public void PlayMusic()
    {
        musicAudioSource.Stop();
        
        //int currentLevel = SceneManager.GetActiveScene().buildIndex - GameManager.instance.ReturnLevelOffset();
        int currentLevel = 0;

        // musicAudioSource.clip = musicClipsSO.musicClipsSO[currentLevel].clip;
        // musicAudioSource.loop = musicClipsSO.musicClipsSO[currentLevel].loop;
        // musicAudioSource.volume = musicClipsSO.musicClipsSO[currentLevel].volume;
        // musicAudioSource.pitch = musicClipsSO.musicClipsSO[currentLevel].pitch;
        // musicAudioSource.outputAudioMixerGroup = musicClipsSO.musicClipsSO[currentLevel].audioMixer;

        musicAudioSource.Play();
    }

    #region --Turret Sounds --

    public void PlayTurretSound()
    {
        PlaySound(platformClips.basicTurret);
    }

    public void PlayLightMissileTurretSound()
    {
        PlaySound(platformClips.lightMissileTurret);
    }

    #endregion
}
