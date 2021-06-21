using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PomodoroBehaviour : MonoBehaviour
{
     static PomodoroTimerModel _pomodoroTimerModel = new PomodoroTimerModel();
    public PomodoroTimerModel PomodoroTimerModel => _pomodoroTimerModel;
    
    public static PomodoroBehaviour Instance { get;  set; }

     void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Restart()
    {
        _pomodoroTimerModel = new PomodoroTimerModel();
        PlayerPrefs.DeleteAll();
    }
    
}
