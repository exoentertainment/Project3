using System;
using UnityEngine;
using TMPro;

public class enemyuilog : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public static enemyuilog instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateLog(string logtext)
    {
        text.text = logtext;
    }
}
