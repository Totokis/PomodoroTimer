using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PomodoroTimerModel
{
    private const int MAX_PAUSE_VALUE = 99;
    private const int MIN_PAUSE_VALUE = 0;
    private const int MAX_WORK_VALUE = 99;
    private const int MIN_WORK_VALUE = 0;
    public int MaxPauseValue => MAX_PAUSE_VALUE;
    public int MinPauseTime => MIN_PAUSE_VALUE;
    public int MaxWorkValue => MAX_WORK_VALUE;
    public int MinWorkValue => MIN_WORK_VALUE;
    
    private bool _isWorkTime = true;
    private bool _isPauseTime = false;
    private bool _paused;
    private int _workTime;
    private int _pauseTime;
    private int _numberOfSessions = 1;
    private int _actualSession = 0;
    private int _minuteCounter;
    private int _secondsCounter;
    public bool IsWorkTime => _isWorkTime;

    public bool IsPauseTime => _isPauseTime;
    public int NumberOfSessions => _numberOfSessions;
    
    public void SetPauseTime(int value)
    {
        if (value > MAX_PAUSE_VALUE)
            _pauseTime = MAX_PAUSE_VALUE;
        else if (value < MIN_PAUSE_VALUE)
            _pauseTime = MIN_PAUSE_VALUE;
        else
        {
            _pauseTime = value;
        }
    }
    public int GetPauseTime()
    {
        return _pauseTime;
    }
    public void SetWorkTime(int value)
    {
        if (value > MAX_WORK_VALUE)
            _workTime = MAX_WORK_VALUE;
        else if (value < MIN_WORK_VALUE)
            _workTime = MIN_WORK_VALUE;
        else
        {
            _workTime = value;
        }
    }
    public int GetWorkTime()
    {
        return _workTime;
    }
    public void IncrementWork(int value)
    {
        _workTime += value;
        SetWorkTime(_workTime);
    }
    public void IncrementPause(int value)
    {
        _pauseTime += value;
        SetPauseTime(_pauseTime);
    }
    public void DecrementPause(int value)
    {
        _pauseTime -= value;
        SetPauseTime(_pauseTime);
    }
    public void DecrementWork(int value)
    {
        _workTime -= value;
        SetWorkTime(_workTime);
    }
    public void StartTimer()
    {
        if (_isWorkTime)
        {
            if (_actualSession == 0)
            {
                _actualSession = 1;
                _minuteCounter = _workTime;
                _secondsCounter = 0;
            }
            else
            {
                _actualSession++;
                _minuteCounter = _workTime;
                _secondsCounter = 0;
            }
        }
        else if(_isPauseTime)
        {
            _minuteCounter = _pauseTime;
            _secondsCounter = 0;
        }
    }
    public int GetMinute()
    {
        return _minuteCounter;
    }
    public int GetSecond()
    {
        return _secondsCounter;
    }
    public double GetActualSession()
    {
        return _actualSession;
    }
    public void PauseTimer()
    {
        _paused = true;
    }
    public void CountDown()
    {
        if (_paused)
            return;

        if (_secondsCounter == 0)
        {
            if (_minuteCounter == 0)
            {
                SwitchState();
            }
            _secondsCounter = 59;
            _minuteCounter--;
        }
        else
        {
            _secondsCounter--;
        }
    }
    private void SwitchState()
    {
        if (_isWorkTime)
        {
            _isWorkTime = false;
            _isPauseTime = true;
        }
        else
        {
            _isPauseTime = false;
            _isWorkTime = true;
        }
        
    }
    
    public void SetTimer(int minute, int second)
    {
        _minuteCounter = minute;
        _secondsCounter = second;
    }
    public void ResumeTimer()
    {
        _paused = false;
    }
}
