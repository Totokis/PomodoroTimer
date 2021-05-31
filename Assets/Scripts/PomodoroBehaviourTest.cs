using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PomodoroBehaviourTest : MonoBehaviour
{
    [SerializeField] int workTime;
    [SerializeField] int pauseTime;
    [SerializeField] int numberOfSessions;
    [SerializeField] Button start;
    [SerializeField] Button pause;
    [SerializeField] Button resume;
    [FormerlySerializedAs("setSecondTo_1")] [SerializeField] Button SetTimeTo_00_01;
    [SerializeField] Text minutes;
    [SerializeField] Text seconds;
    [SerializeField] Text state;
    [SerializeField] Text actualSession;
    [SerializeField] Text done;
    PomodoroTimerModel _pomodoroTimerModel = new PomodoroTimerModel();
    float _nextUpdate;

    private void Awake()
    {
        _pomodoroTimerModel.SetPauseTime(pauseTime);
        _pomodoroTimerModel.SetWorkTime(workTime);
        _pomodoroTimerModel.SetNumberOfSessions(numberOfSessions);
        start.onClick.AddListener(_pomodoroTimerModel.StartTimer);
        pause.onClick.AddListener(_pomodoroTimerModel.PauseTimer);
        resume.onClick.AddListener(_pomodoroTimerModel.ResumeTimer);
        SetTimeTo_00_01.onClick.AddListener(()=>_pomodoroTimerModel.SetTimer(0,1));
    }

    private void Update()
    {
        done.text = _pomodoroTimerModel.Done ? "Done: True" : "Done: False";
        
        if (_pomodoroTimerModel.IsPauseTime)
            state.text = "Pause";
        else if (_pomodoroTimerModel.IsWorkTime)
            state.text = "Work";
        else
            state.text = "No state";

        actualSession.text = $"{_pomodoroTimerModel.GetActualSession()}/{_pomodoroTimerModel.NumberOfSessions}";
        
        if (Time.time >= _nextUpdate)
        {
            _nextUpdate=Mathf.FloorToInt(Time.time)+1;
            _pomodoroTimerModel.CountDown();
        }
        
        minutes.text = _pomodoroTimerModel.GetMinute().ToString();
        seconds.text = _pomodoroTimerModel.GetSecond().ToString();
    }

}
