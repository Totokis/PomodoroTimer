using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PomodoroTimerModel
{
    private const int MAX_PAUSE_VALUE = 99;
    private const int MIN_PAUSE_VALUE = 1;
    private const int MAX_WORK_VALUE = 99;
    private const int MIN_WORK_VALUE = 1;
    public int MaxPauseValue => MAX_PAUSE_VALUE;
    public int MinPauseTime => MIN_PAUSE_VALUE;
    public int MaxWorkValue => MAX_WORK_VALUE;
    public int MinWorkValue => MIN_WORK_VALUE;
    


    private int _workTime;
    private int _pauseTime;
    private int _numberOfSessions;
    private int _minuteCounter;
    private int _secondCounter;
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
    public double GetWorkTime()
    {
        return _workTime;
    }
}
