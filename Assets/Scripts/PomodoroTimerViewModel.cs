using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PomodoroTimerViewModel : MonoBehaviour
{
    [FormerlySerializedAs("pauseResumeButton")]
    [Header("Timer Inputs")]
    [SerializeField]
     Button pauseButton;
    [SerializeField]  Button resumeButton;
    [SerializeField]  Button startButton;
    [SerializeField]  Button muteButton;
    [SerializeField]  Button unmuteButton;
    [SerializeField]  Button endButton;
    [SerializeField]  Button SetTimeTo_00_01;
    [Header("Timer Outputs")]
    [SerializeField]
     TMP_Text minutes;
    [SerializeField]  TMP_Text seconds;
    [SerializeField]  TMP_Text session;
    [SerializeField]  TMP_Text state;
    [FormerlySerializedAs("_backgroundColorizer")]
    [Header("Others")] 
    [SerializeField]
     StateColorizer stateColorizer;
    public bool isRunning = false;
    public UnityEvent timerStarts = new UnityEvent();
    public UnityEvent workDone = new UnityEvent();
    public UnityEvent stateChanged = new UnityEvent();
     float _nextUpdate;
     string _previousState = "Work";
    public string State { get => state.text; set => state.text = value; }
     void Awake()
    {
        ConfigurePauseButton();
        ConfigureResumeButton();
        ConfigureStartButton();
        ConfigureEndButton();
        SetTimeTo_00_01.onClick.AddListener(() => PomodoroBehaviour.Instance.PomodoroTimerModel.SetTimer(0, 1));
        ConfigureStateChanged();
        SetButtons();
        workDone.AddListener(() => {
            

        });
    }
     void Update()
    {
        minutes.text = PomodoroBehaviour.Instance.PomodoroTimerModel.GetMinute().ToString();
        seconds.text = PomodoroBehaviour.Instance.PomodoroTimerModel.GetSecond().ToString();
        session.text = $"{PomodoroBehaviour.Instance.PomodoroTimerModel.GetActualSession()}/{PomodoroBehaviour.Instance.PomodoroTimerModel.NumberOfSessions}";
        if (PomodoroBehaviour.Instance.PomodoroTimerModel.IsPauseTime)
        {
            stateColorizer.SetPauseColor();
            state.text = "Pause";
            if (_previousState != state.text)
            {
                stateChanged.Invoke();
            }
            _previousState = state.text;
        }
        else if (PomodoroBehaviour.Instance.PomodoroTimerModel.IsWorkTime)
        {
            stateColorizer.SetWorkColor();
            state.text = "Work";
            if (_previousState != state.text)
            {
                stateChanged.Invoke();
            }
            _previousState = state.text;
        }
        else if (PomodoroBehaviour.Instance.PomodoroTimerModel.Done)
        {
            state.text = "Done";
            workDone.Invoke();
        }
        else
        {
            state.text = "No state- error";
            _previousState = state.text;
        }


        if (Time.time >= _nextUpdate)
        {
            _nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            PomodoroBehaviour.Instance.PomodoroTimerModel.CountDown();
        }
    }
     void SetButtons()
    {
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }
     void ConfigureStateChanged()
    {
        stateChanged.AddListener(() => {
            Debug.Log("State changed");
            startButton.gameObject.SetActive(true);
            resumeButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(false);
        });
    }
     void ConfigureEndButton()
    {
        endButton.onClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.Done = true;
            PomodoroBehaviour.Instance.Restart();
            _previousState = "Work";
            workDone.Invoke();
            SetButtons();
        });
    }
     void ConfigureStartButton()
    {
        startButton.onClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.StartTimer();
            resumeButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            startButton.gameObject.SetActive(false);

        });
    }
     void ConfigureResumeButton()
    {
        resumeButton.onClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.ResumeTimer();
            resumeButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
        });
    }
     void ConfigurePauseButton()
    {
        pauseButton.onClick.AddListener(() => {
            PomodoroBehaviour.Instance.PomodoroTimerModel.PauseTimer();
            resumeButton.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
        });
    }
}