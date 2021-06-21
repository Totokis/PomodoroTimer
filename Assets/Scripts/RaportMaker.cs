using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RaportMaker : MonoBehaviour
{
   static public RaportMaker Instance { get;  set; }
   
   [SerializeField] PomodoroTimerViewModel _pomodoroTimerViewModel;
   [SerializeField] Button startButton;
   [SerializeField] Button pauseButton;
   DateTime _startTime;
   readonly string _raport = "";
   public string Raport=>_raport;

    void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         _pomodoroTimerViewModel.workDone.AddListener(WorkDone);
         startButton.onClick.AddListener(() => {
            _startTime = DateTime.Now;
         });
      }
      else
      {
         Destroy(this);
      }
     
   }

    void WorkDone()
   {
      var numberOfSessions = PomodoroBehaviour.Instance.PomodoroTimerModel.NumberOfSessions;
      var completedSessions = PomodoroBehaviour.Instance.PomodoroTimerModel.GetActualSession();
      var startTime = $"start time: {this._startTime.Hour}:{this._startTime.Minute}";
      var totalWorkTime = DateTime.Now - this._startTime;
      var totalTime = $"total time: {totalWorkTime.Hours}:{totalWorkTime.Minutes}";
      Logger.Instance.SetText($"Number of sessions: {numberOfSessions} \n completed sessions: {completedSessions} \n {startTime} \n {totalTime}");
      var doneEventHandler = new CalendarEvent.DoneEventHandler((_) => {
         Debug.Log("Done");
      });
      if (Application.isEditor)
         return;
      
#if PLATFORM_ANDROID
      if (!Permission.HasUserAuthorizedPermission("android.permission.WRITE_CALENDAR"))
      {
         Permission.RequestUserPermission("android.permission.WRITE_CALENDAR");
      }
      if (!Permission.HasUserAuthorizedPermission("android.permission.READ_CALENDAR"))
      {
         Permission.RequestUserPermission("android.permission.READ_CALENDAR");
      }
#endif
      if (Permission.HasUserAuthorizedPermission("android.permission.WRITE_CALENDAR"))
      {
         CalendarEvent.AddEvent("Today you completed your work !", this._startTime, DateTime.Now, false, doneEventHandler);
      }else
      {
         Debug.Log("Permission WRITE_CALENDAR Denied");
      }
   }
}
