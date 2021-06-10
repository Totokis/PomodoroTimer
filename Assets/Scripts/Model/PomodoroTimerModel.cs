
public class PomodoroTimerModel
{
    private const int MAXPauseValue = 99;
    private const int MINPauseValue = 1;
    private const int MAXWorkValue = 99;
    private const int MINWorkValue = 1;
    public int MaxPauseValue => MAXPauseValue;
    public int MinPauseTime => MINPauseValue;
    public int MaxWorkValue => MAXWorkValue;
    public int MinWorkValue => MINWorkValue;
    
    private bool _isWorkTime = true;
    private bool _isPauseTime = false;
    private bool _done = false;
    private bool _paused = true;
    private int _workTime;
    private int _pauseTime;
    private int _numberOfSessions = 1;
    private int _actualSession = 0;
    private int _minuteCounter = 0;
    private int _secondsCounter = 0;
    public bool IsWorkTime
    {
        get => _isWorkTime;
        set
        {
            _isWorkTime = value;
            _isPauseTime = !value;
        }
    }
    public bool IsPauseTime  {
        get => _isPauseTime;
        set
        {
            _isPauseTime = value;
            _isWorkTime = !value;
        }
    }
    public bool Done { get => _done; set
        {
            _done = value;
            if (value)
                _paused = true;
        } 
    }
    public bool Paused => _paused;
    public int NumberOfSessions => _numberOfSessions;

    public PomodoroTimerModel()
    {
        _workTime = 1;
        _pauseTime = 1;
    }
    
    public void SetPauseTime(int value)
    {
        if (value > MAXPauseValue)
            _pauseTime = MAXPauseValue;
        else if (value < MINPauseValue)
            _pauseTime = MINPauseValue;
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
        if (value > MAXWorkValue)
            _workTime = MAXWorkValue;
        else if (value < MINWorkValue)
            _workTime = MINWorkValue;
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
        _paused = false;
        if (_minuteCounter == 0 && _secondsCounter == 0)
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
    }
    private void ResetTimer()
    {
        _actualSession = 0;
        _isWorkTime = true;
        _isPauseTime = false;
    }
    public int GetMinute()
    {
        return _minuteCounter;
    }
    public int GetSecond()
    {
        return _secondsCounter;
    }
    public int GetActualSession()
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
        if (_actualSession == 0)
            return;

        if (_secondsCounter == 0)
        {
            if (_minuteCounter == 0)
            {
                SwitchState();
                return;
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
            if (_actualSession == _numberOfSessions)
                _done = true;
            _isWorkTime = false;
            _isPauseTime = true;
        }
        else
        {
            _isPauseTime = false;
            _isWorkTime = true;
        }
        _paused = true;
    }
    
    public void SetTimer(int minute, int second)
    {
        _minuteCounter = minute<0?0:minute;
        _secondsCounter = second<0?1:second;
    }
    public void ResumeTimer()
    {
        _paused = false;
    }
    public void SetNumberOfSessions(int value)
    {
        _numberOfSessions = value < 1 ? 1 : value;
    }
    public void SetActualSession(int value)
    {
        _actualSession = value < 1 ? 1 : value;

    }
}