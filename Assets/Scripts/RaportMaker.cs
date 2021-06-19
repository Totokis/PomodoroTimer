using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class RaportMaker : MonoBehaviour
{
   [SerializeField] PomodoroTimerViewModel _pomodoroTimerViewModel;
   [SerializeField] Button startButton;
   [SerializeField] Button pauseButton;
   DateTime startTime;
   void Awake()
   {
      _pomodoroTimerViewModel.workDone.AddListener(WorkDone);
      startButton.onClick.AddListener(() => {
         startTime = DateTime.Now;
      });
   }
   
   void WorkDone()
   {
      var numberOfSessions = PomodoroBehaviour.Instance.PomodoroTimerModel.NumberOfSessions;
      var completedSessions = PomodoroBehaviour.Instance.PomodoroTimerModel.GetActualSession();
      var startTime = $"start time: {this.startTime.Hour}:{this.startTime.Minute}";
      var totalWorkTime = DateTime.Now - this.startTime;
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
         CalendarEvent.AddEvent("Today you completed your work !", this.startTime, DateTime.Now, false, doneEventHandler);
      }else
      {
         Debug.Log("Permission WRITE_CALENDAR Denied");
      }
   }
   
void OnApplicationFocus(bool hasFocus)
{
   if (Permission.HasUserAuthorizedPermission("android.permission.WRITE_CALENDAR"))
   {
      Debug.Log("Permission WRITE_CALENDAR Allowed");
   }else
   {
      Debug.Log("Permission WRITE_CALENDAR Denied");
   }
   if (Permission.HasUserAuthorizedPermission("android.permission.READ_CALENDAR"))
   {
      Debug.Log("Permission READ_CALENDAR Allowed");
   }else
   {
      Debug.Log("Permission READ_CALENDAR Denied");
   }
}
   
}
