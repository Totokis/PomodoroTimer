using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ViewUI;

public class PomodoroTimeSetterViewModel : MonoBehaviour
{
    [Header("Timer Setter Inputs")]
    [SerializeField] Button startTimer;
    [SerializeField] LongClickButton incrementWork;
    [SerializeField] LongClickButton incrementPause;
    [SerializeField] LongClickButton decrementWork;
    [SerializeField] LongClickButton decrementPause;
    [SerializeField] TMP_InputField numberOfSessions;
    [Header("Timer Setter Outputs")]
    [SerializeField] TMP_Text workTime;
    [SerializeField] TMP_Text pauseTime;
    

    void Awake()
    {
        startTimer.onClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.StartTimer();
        });
        incrementPause.onClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.IncrementPause(1);
        });
        incrementPause.onLongClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.IncrementPause(10);
        });
        incrementWork.onClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.IncrementWork(1);
        });
        incrementWork.onLongClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.IncrementWork(10);
        });
        decrementWork.onClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.DecrementWork(1);
        });
        decrementWork.onLongClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.DecrementWork(10);
        });
        decrementPause.onClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.DecrementPause(1);
        });
        decrementPause.onLongClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.DecrementPause(10);
        });
        
        numberOfSessions.onValueChanged.AddListener(
            value=> 
            {
                PomodoroBehaviour.Instance.PomodoroTimerModel.SetNumberOfSessions(Int32.Parse(value)); 
            }
            );
    }

    void Update()
    {
        workTime.text = PomodoroBehaviour.Instance.PomodoroTimerModel.GetWorkTime().ToString();
        pauseTime.text = PomodoroBehaviour.Instance.PomodoroTimerModel.GetPauseTime().ToString();
        numberOfSessions.text = PomodoroBehaviour.Instance.PomodoroTimerModel.NumberOfSessions.ToString();
    }
}
