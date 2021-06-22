using UnityEngine;
using UnityEngine.UI;

public class AlarmSound : MonoBehaviour
{
    [SerializeField]  AudioSource _audioSource;
    [SerializeField]  Button startButton;
    [SerializeField]  Button stopButton;
    [SerializeField]  Button endButton;
    [SerializeField]  PomodoroTimerViewModel _pomodoroTimer;
    [SerializeField]  Slider volumeSlider;

     void Awake()
    {
        volumeSlider.onValueChanged.AddListener(value => {
            _audioSource.volume = value;
        });
        _pomodoroTimer.stateChanged.AddListener(() => {
            if(PomodoroBehaviour.Instance.PomodoroTimerModel.GetSecond()==0)
                _audioSource.Play();
        });
        _pomodoroTimer.workDone.AddListener((() => {
            _audioSource.enabled = false;
        }));
        stopButton.onClick.AddListener(_audioSource.Stop);
        startButton.onClick.AddListener(() => {
                _audioSource.enabled = true;
            }
        );
    }
}
