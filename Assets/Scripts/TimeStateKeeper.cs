using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.UI;
using Unosquare.Swan;

public class TimeStateKeeper : MonoBehaviour
{
    [SerializeField] TMP_Text logger;
    [SerializeField] Button startButton;
    [SerializeField] Button pauseButton;
    [SerializeField] PomodoroTimerViewModel pomodoroTimer;
    bool _isWorking = false;

    private void Start()
    {
        var c = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
    }
    
    void Awake()
    {
        Application.runInBackground = false;
        startButton.onClick.AddListener((() => {
            _isWorking = true;
        }));
        
    }
    void ReadFromPlayerPrefs()
    {
        
        if (PlayerPrefs.GetInt("IsAlreadyWorking") != 1)
            return;

        var time = new DateTime(long.Parse(PlayerPrefs.GetString("Time")));
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
            logger.text = "Not paused";
            var timeDifference = DateTime.Now - time;
            var restTime = new TimeSpan(0, minutes, seconds);
            var newMinutes = minutes - timeDifference.Minutes;
            var newSeconds = seconds - timeDifference.Seconds;
            logger.text=$"Your application was closed {timeDifference.Minutes}:{timeDifference.Seconds} and you have {newMinutes}:{newSeconds}";
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetTimer(newMinutes >= 0 ? newMinutes : 0, newSeconds >= 0 ? newSeconds : 1);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetNumberOfSessions(numberOfSessions);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetActualSession(actualSession);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetPauseTime(pauseTime);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetWorkTime(workTime);
            pomodoroTimer.State = state;
            startButton.onClick.Invoke();
        }
        else
        {
            logger.text = "Paused";
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetTimer(minutes,seconds);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetNumberOfSessions(numberOfSessions);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetActualSession(actualSession);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetPauseTime(pauseTime);
            PomodoroBehaviour.Instance.PomodoroTimerModel.SetWorkTime(workTime);
            PomodoroBehaviour.Instance.PomodoroTimerModel.PauseTimer();
            pomodoroTimer.State = state;
            startButton.onClick.Invoke();
            pauseButton.onClick.Invoke();
        }

    }

    void OnEnable()
    {
        CancelNotifications();
        ReadFromPlayerPrefs();
    }
    
    void OnApplicationPause(bool pauseStatus)
    {
      
        if (pauseStatus)
        {
            CancelNotifications();
            MakeNotification();
            SaveToPlayerPrefs();
        }
        else
        {
            CancelNotifications();
            ReadFromPlayerPrefs();
        }
    }
    

    void OnApplicationFocus(bool hasFocus)
    {
       
        if (!hasFocus)
        {
            CancelNotifications();
            MakeNotification();
            SaveToPlayerPrefs();
        }
        else
        {
            CancelNotifications();
            ReadFromPlayerPrefs();
        }
    }

    void SaveToPlayerPrefs()
    {
        if (_isWorking&&!PomodoroBehaviour.Instance.PomodoroTimerModel.Done)
        {
            PlayerPrefs.SetInt("IsAlreadyWorking",1);
            PlayerPrefs.SetString("Time",DateTime.Now.Ticks.ToString());
            PlayerPrefs.SetInt("Minutes",PomodoroBehaviour.Instance.PomodoroTimerModel.GetMinute());
            PlayerPrefs.SetInt("Seconds",PomodoroBehaviour.Instance.PomodoroTimerModel.GetSecond());
            PlayerPrefs.SetInt("WorkTime",PomodoroBehaviour.Instance.PomodoroTimerModel.GetWorkTime());
            PlayerPrefs.SetInt("PauseTime",PomodoroBehaviour.Instance.PomodoroTimerModel.GetPauseTime());
            PlayerPrefs.SetInt("ActualSession",PomodoroBehaviour.Instance.PomodoroTimerModel.GetActualSession());
            PlayerPrefs.SetInt("NumberOfSessions",PomodoroBehaviour.Instance.PomodoroTimerModel.NumberOfSessions);
            PlayerPrefs.SetString("State",pomodoroTimer.State);
            PlayerPrefs.SetInt("Paused",PomodoroBehaviour.Instance.PomodoroTimerModel.Paused?1:0);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.DeleteAll();
        }
        Application.Quit();
    }
    
    private static void MakeNotification()
    {
        if (!PomodoroBehaviour.Instance.PomodoroTimerModel.Paused)
        {
            var minutes = PomodoroBehaviour.Instance.PomodoroTimerModel.GetMinute();
            var seconds = PomodoroBehaviour.Instance.PomodoroTimerModel.GetSecond();
            var notification = new AndroidNotification();
            notification.Title = "State changed";
            notification.Text = "Click me";
            notification.FireTime = System.DateTime.Now.AddSeconds(minutes * 60 + seconds);
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }
    }

    private static void CancelNotifications()
    {

        AndroidNotificationCenter.CancelAllNotifications();
    }
}
