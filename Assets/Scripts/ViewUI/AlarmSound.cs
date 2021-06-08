using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlarmSound : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] Button stopButton;
    [SerializeField] PomodoroTimerViewModel _pomodoroTimer;

    void Awake()
    {
        _pomodoroTimer.stateChanged.AddListener(_audioSource.Play);
        stopButton.onClick.AddListener(_audioSource.Pause);
    }
}
