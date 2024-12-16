using System.Collections;
using UnityEngine;

public class TurnObjectOffTimer : MonoBehaviour
{
    [SerializeField] private float disableTimer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DisableTimerRoutine());
    }

    IEnumerator DisableTimerRoutine()
    {
        yield return new WaitForSeconds(disableTimer);
        
        GameManager.instance.LoadNextLevelButton();
    }
}
