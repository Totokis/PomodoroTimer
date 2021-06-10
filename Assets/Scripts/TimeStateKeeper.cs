using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeStateKeeper : MonoBehaviour
{
    [SerializeField] PomodoroTimerViewModel _pomodoroTimerViewModel;
    [SerializeField] Button startButton;
    [SerializeField] Button pauseButton;
    [SerializeField] Button endButton; 
    bool stateChanged = false;
    [SerializeField] bool isWorking = false;
    bool isStateLoaded = false;
    bool isStateSaved = false;
    static string loggerMessage;

    private void Awake()
    {
        endButton.onClick.AddListener(() => {
            isWorking = false;
        });
        
        startButton.onClick.AddListener(() => {
            isWorking = true;
        });
        
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveStateOnPause();
        }
        else
        {
            LoadState();
            StartCoroutine(TimerSaveState(15));
        }
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveStateOnPause();
        }
        else
        {
            LoadState();
            StartCoroutine(TimerSaveState(15));
        }
    }
    private IEnumerator TimerSaveState(int seconds)
    {
        while (!isStateSaved)
        {
            yield return new WaitForSeconds(seconds);
            yield return SaveStateOnCoroutine();
        }
    }

    private void LoadState()
    { 
        loggerMessage = "";
        if (isStateLoaded)
            return;

        // if (PomodoroBehaviour.Instance.PomodoroTimerModel.Done)
        // {
        //     loggerMessage += "Done: True\n";
        //     return;
        // }
        // loggerMessage += "Done: False\n";
        //
        // if (PlayerPrefs.GetInt("IsSaved") == 0)
        // {
        //     loggerMessage += "IsSaved: False\n";
        //     return;
        // }
        // loggerMessage += "IsSaved: True\n";
        //
        // if (PlayerPrefs.GetInt("IsWorking") == 0)
        // {
        //     loggerMessage += "IsWorking: False\n";
        //     return;
        // }
        // loggerMessage += "IsWorking: True\n";
        //
        // if (isFreshStarted)
        // {
        //     loggerMessage += "IsFreshStarted: True\n";
        //     startButton.onClick.Invoke();
        //     isFreshStarted = false;
        // }
        //
        // LoadFromPlayerPrefs();
        
        if (PlayerPrefs.GetInt("IsWorking") == 0)
            return;
        
        var time = new DateTime(long.Parse(PlayerPrefs.GetString("SaveTime")));
        var minutes = PlayerPrefs.GetInt("Minutes");
        var seconds = PlayerPrefs.GetInt("Seconds");
        var workTime = PlayerPrefs.GetInt("WorkTime");
        var pauseTime = PlayerPrefs.GetInt("PauseTime");
        var actualSession = PlayerPrefs.GetInt("ActualSession");
        var numberOfSessions = PlayerPrefs.GetInt("NumberOfSessions");
        var state = PlayerPrefs.GetString("State");
        var paused = PlayerPrefs.GetInt("Paused");

        if (paused == 0)
        {
            var timeDifference = DateTime.Now - time;
            var restTime = new TimeSpan(0, minutes, seconds);
            var newMinutes = minutes - timeDifference.Minutes;
            var newSeconds = seconds - timeDifference.Seconds;
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetNumberOfSessions(numberOfSessions);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetActualSession(actualSession);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetPauseTime(pauseTime);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetWorkTime(workTime);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetTimer(newMinutes >= 0 ? newMinutes : 0, newSeconds >= 0 ? newSeconds : 1);
            PomodoroBehaviour.Instance.PomodoroTimerModel.IsWorkTime = Convert.ToBoolean(PlayerPrefs.GetInt("IsWorkTime"));
            startButton.onClick.Invoke();
        }
        else
        {
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetNumberOfSessions(numberOfSessions);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetActualSession(actualSession);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetPauseTime(pauseTime);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetWorkTime(workTime);
            PomodoroBehaviour.Instance.PomodoroTimerModel.PauseTimer();
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetTimer(minutes,seconds);
            PomodoroBehaviour.Instance.PomodoroTimerModel.IsWorkTime = Convert.ToBoolean(PlayerPrefs.GetInt("IsWorkTime"));
            
            startButton.onClick.Invoke();
            pauseButton.onClick.Invoke();
        }

        isStateLoaded = true;
        isStateSaved = false;
        PlayerPrefs.DeleteAll();
    }
    private void LoadFromPlayerPrefs()
    {
        //Get rest of time
        var minutes = PlayerPrefs.GetInt("Minutes");
        var seconds = PlayerPrefs.GetInt("Seconds");
        //Get save time
        if(PlayerPrefs.GetInt("Paused")==0)
        {
            var time = new DateTime(long.Parse(PlayerPrefs.GetString("SaveTime")));
            //Get difference
            var difference = DateTime.Now - time;
            var restOfTime = new TimeSpan(0, minutes, seconds);

            var newTime = restOfTime - difference;
            loggerMessage += $"Saved at: {time.ToShortTimeString()}\n" +
                             $"You had left {minutes} minutes {seconds} seconds\n" +
                             $"You've paused app for {difference}\n" +
                             $"So now you have {newTime} left";
            minutes = newTime.Minutes;
            seconds = newTime.Seconds;
        }
        else
        {
            pauseButton.onClick.Invoke();
        }
        
        PomodoroBehaviour.Instance.PomodoroTimerModel.SetTimer(minutes, seconds);
        PomodoroBehaviour.Instance.PomodoroTimerModel.SetActualSession(PlayerPrefs.GetInt("ActualSession"));
        PomodoroBehaviour.Instance.PomodoroTimerModel.SetNumberOfSessions(PlayerPrefs.GetInt("NumberOfSessions"));
        PomodoroBehaviour.Instance.PomodoroTimerModel.SetPauseTime(PlayerPrefs.GetInt("PauseTime"));
        PomodoroBehaviour.Instance.PomodoroTimerModel.SetWorkTime(PlayerPrefs.GetInt("WorkTime"));
        Logger.Instance.Text = loggerMessage;
    }
    private void SaveStateOnPause()
    {
        if (PomodoroBehaviour.Instance.PomodoroTimerModel.Done)
            return;
        SetIntoPlayerPrefs();
        isStateLoaded = false;
        isStateSaved = true;
    }

    private IEnumerator SaveStateOnCoroutine()
    {
        if (PomodoroBehaviour.Instance.PomodoroTimerModel.Done)
            yield break;
        SetIntoPlayerPrefs();
    }
    private void SetIntoPlayerPrefs()
    {
        PlayerPrefs.SetInt("IsSaved", 1);
        PlayerPrefs.SetInt("IsWorking", isWorking ? 1 : 0);
        PlayerPrefs.SetInt("Minutes", PomodoroBehaviour.Instance.PomodoroTimerModel.GetMinute());
        PlayerPrefs.SetInt("Seconds", PomodoroBehaviour.Instance.PomodoroTimerModel.GetSecond());
        PlayerPrefs.SetInt("NumberOfSessions", PomodoroBehaviour.Instance.PomodoroTimerModel.NumberOfSessions);
        PlayerPrefs.SetInt("ActualSession", PomodoroBehaviour.Instance.PomodoroTimerModel.GetActualSession());
        PlayerPrefs.SetInt("Paused", PomodoroBehaviour.Instance.PomodoroTimerModel.Paused?1:0);
        PlayerPrefs.SetInt("WorkTime", PomodoroBehaviour.Instance.PomodoroTimerModel.GetWorkTime());
        PlayerPrefs.SetInt("PauseTime", PomodoroBehaviour.Instance.PomodoroTimerModel.GetPauseTime());
        PlayerPrefs.SetString("SaveTime",DateTime.Now.Ticks.ToString());
        PlayerPrefs.SetInt("IsWorkTime",PomodoroBehaviour.Instance.PomodoroTimerModel.IsWorkTime?1:0);
        PlayerPrefs.SetInt("StateChanged",stateChanged?1:0);
        PlayerPrefs.Save();
    }
}
