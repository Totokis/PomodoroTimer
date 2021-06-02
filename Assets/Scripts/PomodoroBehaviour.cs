using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PomodoroBehaviour : MonoBehaviour
{
    private static PomodoroTimerModel _pomodoroTimerModel = new PomodoroTimerModel();
    public PomodoroTimerModel PomodoroTimerModel => _pomodoroTimerModel;
    public static PomodoroBehaviour Instance { get; private set; }

    private void Awake()
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
    
    
}
