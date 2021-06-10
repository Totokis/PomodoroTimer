using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeStateKeeper : MonoBehaviour
{
    [SerializeField]  Button startButton;
    [SerializeField]  Button pauseButton;
    [SerializeField]  Button endButton;
    [SerializeField]  bool isWorking;
     bool isStateLoaded;
     bool isStateSaved;
     readonly bool stateChanged = false;

     void Awake()
    {
        endButton.onClick.AddListener(() => {
            isWorking = false;
        });

        startButton.onClick.AddListener(() => {
            isWorking = true;
        });

    }

     void OnApplicationFocus(bool hasFocus)
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

     void OnApplicationPause(bool pauseStatus)
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
     IEnumerator TimerSaveState(int seconds)
    {
        while (!isStateSaved)
        {
            yield return new WaitForSeconds(seconds);
            yield return SaveStateOnCoroutine();
        }
    }

     void LoadState()
    {
        if (isStateLoaded)
            return;


        if (PlayerPrefs.GetInt("IsWorking") == 0)
            return;

        var saveTime = new DateTime(long.Parse(PlayerPrefs.GetString("SaveTime")));
        var minutesRest = PlayerPrefs.GetInt("Minutes");
        var secondsRest = PlayerPrefs.GetInt("Seconds");
        var workTime = PlayerPrefs.GetInt("WorkTime");
        var pauseTime = PlayerPrefs.GetInt("PauseTime");
        var actualSession = PlayerPrefs.GetInt("ActualSession");
        var numberOfSessions = PlayerPrefs.GetInt("NumberOfSessions");
        var state = PlayerPrefs.GetString("State");
        var paused = PlayerPrefs.GetInt("Paused");

        SetDataIntoTimer(numberOfSessions, actualSession, pauseTime, workTime);
        if (paused == 0)
        {
            TimerInProgress(saveTime, minutesRest, secondsRest);
        }
        else
        {
            TimerInPause(minutesRest, secondsRest);
        }

        isStateLoaded = true;
        isStateSaved = false;
        PlayerPrefs.DeleteAll();
    }
      void TimerInPause(int minutes, int seconds)
     {
         PomodoroBehaviour.Instance.PomodoroTimerModel.PauseTimer();
         PomodoroBehaviour.Instance.PomodoroTimerModel.SetTimer(minutes >= 0 ? minutes : 0, seconds >= 0 ? seconds : 1);
         startButton.onClick.Invoke();
         pauseButton.onClick.Invoke();
     }
      void TimerInProgress(DateTime time, int minutes, int seconds)
     {
         var timeDifference = DateTime.Now - time;
         minutes = minutes - timeDifference.Minutes;
         seconds = seconds - timeDifference.Seconds;
         PomodoroBehaviour.Instance.PomodoroTimerModel.SetTimer(minutes >= 0 ? minutes : 0, seconds >= 0 ? seconds : 1);
         startButton.onClick.Invoke();
     }
      static void SetDataIntoTimer(int numberOfSessions, int actualSession, int pauseTime, int workTime)
     {

         PomodoroBehaviour.Instance.PomodoroTimerModel.SetNumberOfSessions(numberOfSessions);
         PomodoroBehaviour.Instance.PomodoroTimerModel.SetActualSession(actualSession);
         PomodoroBehaviour.Instance.PomodoroTimerModel.SetPauseTime(pauseTime);
         PomodoroBehaviour.Instance.PomodoroTimerModel.SetWorkTime(workTime);
         PomodoroBehaviour.Instance.PomodoroTimerModel.IsWorkTime = Convert.ToBoolean(PlayerPrefs.GetInt("IsWorkTime"));
     }
     void SaveStateOnPause()
    {
        if (PomodoroBehaviour.Instance.PomodoroTimerModel.Done)
            return;

        SetIntoPlayerPrefs();
        isStateLoaded = false;
        isStateSaved = true;
    }

     IEnumerator SaveStateOnCoroutine()
    {
        if (PomodoroBehaviour.Instance.PomodoroTimerModel.Done)
            yield break;

        SetIntoPlayerPrefs();
    }
     void SetIntoPlayerPrefs()
    {
        PlayerPrefs.SetInt("IsSaved", 1);
        PlayerPrefs.SetInt("IsWorking", isWorking ? 1 : 0);
        PlayerPrefs.SetInt("Minutes", PomodoroBehaviour.Instance.PomodoroTimerModel.GetMinute());
        PlayerPrefs.SetInt("Seconds", PomodoroBehaviour.Instance.PomodoroTimerModel.GetSecond());
        PlayerPrefs.SetInt("NumberOfSessions", PomodoroBehaviour.Instance.PomodoroTimerModel.NumberOfSessions);
        PlayerPrefs.SetInt("ActualSession", PomodoroBehaviour.Instance.PomodoroTimerModel.GetActualSession());
        PlayerPrefs.SetInt("Paused", PomodoroBehaviour.Instance.PomodoroTimerModel.Paused ? 1 : 0);
        PlayerPrefs.SetInt("WorkTime", PomodoroBehaviour.Instance.PomodoroTimerModel.GetWorkTime());
        PlayerPrefs.SetInt("PauseTime", PomodoroBehaviour.Instance.PomodoroTimerModel.GetPauseTime());
        PlayerPrefs.SetString("SaveTime", DateTime.Now.Ticks.ToString());
        PlayerPrefs.SetInt("IsWorkTime", PomodoroBehaviour.Instance.PomodoroTimerModel.IsWorkTime ? 1 : 0);
        PlayerPrefs.SetInt("StateChanged", stateChanged ? 1 : 0);
        PlayerPrefs.Save();
    }
}
