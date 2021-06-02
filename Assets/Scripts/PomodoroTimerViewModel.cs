using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PomodoroTimerViewModel : MonoBehaviour
{
    [FormerlySerializedAs("pauseResumeButton")]
    [Header("Timer Inputs")] 
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button startButton;
    [SerializeField] Button muteButton;
    [SerializeField] Button unmuteButton;
    [SerializeField] Button endButton;
    [SerializeField] Button SetTimeTo_00_01;
    [Header("Timer Outputs")]
    [SerializeField] TMP_Text minutes;
    [SerializeField] TMP_Text seconds;
    [SerializeField] TMP_Text session;
    [SerializeField] TMP_Text state;
    UnityEvent stateChanged = new UnityEvent();
    UnityEvent workDone = new UnityEvent();
    string _previousState = "Work";
    float _nextUpdate;
    void Awake()
    {
        pauseButton.onClick.AddListener(()=> {
            PomodoroBehaviour.Instance.PomodoroTimerModel.PauseTimer();
            resumeButton.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
        });
        resumeButton.onClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.ResumeTimer();
            resumeButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
        });
        startButton.onClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.StartTimer();
            resumeButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            startButton.gameObject.SetActive(false);
            
        });
        SetTimeTo_00_01.onClick.AddListener(()=>PomodoroBehaviour.Instance.PomodoroTimerModel.SetTimer(0,1));
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        
        stateChanged.AddListener((() => {
            startButton.gameObject.SetActive(true);
            resumeButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(false);
        }));
    }
    void Update()
    {
        minutes.text = PomodoroBehaviour.Instance.PomodoroTimerModel.GetMinute().ToString();
        seconds.text = PomodoroBehaviour.Instance.PomodoroTimerModel.GetSecond().ToString();
        session.text = $"{PomodoroBehaviour.Instance.PomodoroTimerModel.GetActualSession()}/{PomodoroBehaviour.Instance.PomodoroTimerModel.NumberOfSessions}";
        if (PomodoroBehaviour.Instance.PomodoroTimerModel.IsPauseTime)
        {
            state.text = "Pause";
            if (_previousState != state.text)
            {
                stateChanged.Invoke();
            }
            _previousState = state.text;
        }
        else if (PomodoroBehaviour.Instance.PomodoroTimerModel.IsWorkTime)
        {
            state.text = "Work";
            if (_previousState != state.text)
            {
                stateChanged.Invoke();
            }
            _previousState = state.text;
        }
        else if (PomodoroBehaviour.Instance.PomodoroTimerModel.Done)
        {
            state.text = "Done";
            workDone.Invoke();
        }
        else
        {
            state.text = "No state- error";
            _previousState = state.text;
        }
        
        
        if (Time.time >= _nextUpdate)
        {
            _nextUpdate=Mathf.FloorToInt(Time.time)+1;
            PomodoroBehaviour.Instance.PomodoroTimerModel.CountDown();
        }
    }
    
}
