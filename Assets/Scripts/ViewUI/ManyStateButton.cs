using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ManyStateButton : MonoBehaviour
{
    public List<Button> buttons;
    public Button eventStateButton;
    private List<UnityEvent> _states = new List<UnityEvent>();
    public List<UnityEvent> States => _states;

    void Awake()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].onClick.AddListener(()=>{_states[i].Invoke();});
        }
        if ((_states.Count == buttons.Count + 1) && (eventStateButton != null))
        {
            eventStateButton.onClick.AddListener(()=>{_states.Last().Invoke();});
        }

        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        buttons.First().gameObject.SetActive(true);
    }

    void Update()
    {
        if (PomodoroBehaviour.Instance.PomodoroTimerModel.IsPauseTime)
        {
            
        }
    }

}
