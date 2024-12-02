using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region -- Serialized Fields

    [Header("Variables")] 
    [SerializeField] private int musicTrack;

    [SerializeField] private GameObject LoadNextLevelWindow;

    #endregion

    public static GameManager instance;

    public GameObject lastEnemy;
    private bool isLastEnemySet;
    private bool isNextLevelButtonDisplayed;

    private void Start()
    {
        PlayMusic();
        
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
    
        instance = this;
    }

    private void Update()
    {
        CheckStatusLastEnemy();
    }

    void PlayMusic()
    {
        AudioManager.instance.PlayMusic(musicTrack);
    }

    public void AssignLastEnemy(GameObject enemy)
    {
        lastEnemy = enemy;
        isLastEnemySet = true;
    }
    
    void CheckStatusLastEnemy()
    {
        if (lastEnemy == null && isLastEnemySet && !isNextLevelButtonDisplayed)
        {
            isNextLevelButtonDisplayed = true;
            LoadNextLevelButton();
        }
    }
    
    public void LoadNextLevelButton()
    {
        LoadNextLevelWindow.SetActive(true);
    }
}
