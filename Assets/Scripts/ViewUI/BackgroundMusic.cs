using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] Button muteButton;
    [SerializeField] Button playButton;
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button stopStartButton;
    [SerializeField] PomodoroTimerViewModel pomodoroTimer;
    bool isPauseTime = false;

    void Awake()
    {
        muteButton.onClick.AddListener(() => {
            _audioSource.mute = !_audioSource.mute;
        });
        pauseButton.onClick.AddListener((() => {
            _audioSource.Pause();
        }));
        resumeButton.onClick.AddListener(() => {
            _audioSource.UnPause();
        });
        playButton.onClick.AddListener(_audioSource.Play);
        
        stopStartButton.onClick.AddListener(() => {
            if (isPauseTime)
            {
                _audioSource.Pause();
            }
            else
            {
                _audioSource.UnPause();
            }
        });
        
        pomodoroTimer.stateChanged.AddListener(() => {
            if (pomodoroTimer.State == "Pause")
                isPauseTime = true;
            else if (pomodoroTimer.State == "Work")
                isPauseTime = false;
        });
    }
}
