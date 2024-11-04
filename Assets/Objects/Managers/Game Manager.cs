using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region -- Serialized Fields

    [Header("Variables")] 
    [SerializeField] private int musicTrack;

    #endregion

    public static GameManager instance;

    private void Start()
    {
        PlayMusic();
    }

    void PlayMusic()
    {
        AudioManager.instance.PlayMusic(musicTrack);
    }
}
