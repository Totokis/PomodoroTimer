using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
      
      Debug.Log($"Number of sessions: {numberOfSessions} \n completed sessions: {completedSessions} \n {startTime} \n {totalTime}");
   }
   
}
