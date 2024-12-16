using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region -- Serialized Fields

    [Header("Variables")] 
    [SerializeField] private int musicTrack;
    
    [SerializeField] private GameObject LoadNextLevelWindow;
    [SerializeField] private GameObject GameOverWindow;
    [SerializeField] private GameObject DemoOverWindow;

    #endregion

    public static GameManager instance;

    public GameObject lastEnemy;
    private bool isLastEnemySet;
    private bool isNextLevelButtonDisplayed;
    bool isGameOver;
    private int NumPlanets;
    
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

    public void AddPlanet()
    {
        NumPlanets++;
    }

    public void RemovePlanet()
    {
        NumPlanets--;

        if (NumPlanets == 0 && !isGameOver)
        {
            isGameOver = true;
            AudioManager.instance.PlayGameOver();
        }
    }

    void LoadGameOverWindow()
    {
        GameOverWindow.SetActive(true);
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
        StartCoroutine(LoadNextLevelRoutine());
    }

    IEnumerator LoadNextLevelRoutine()
    {
        yield return new WaitForSeconds(5);
        
        LoadNextLevelWindow.SetActive(true);
        AudioManager.instance.PlayLevelCleared();
    }
}
