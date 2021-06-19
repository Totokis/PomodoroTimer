using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PomodoroViewLogic : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] GameObject pomodoroTimeSetter;
    [SerializeField] GameObject pomodoroTimer;
    [SerializeField] GameObject twoPanels;
    [SerializeField] Button startButton;
    [SerializeField] Button endButton;

    void Awake()
    {
        startButton.onClick.AddListener(SwitchToTimer);
        endButton.onClick.AddListener(SwitchToSetter);
    }
    void SwitchToSetter()
    {
        pomodoroTimeSetter.gameObject.SetActive(true);
        LeanTween.moveX(twoPanels, 0, 0.3f);
        StartCoroutine(SetToFalse(pomodoroTimer)); 
    }
    private IEnumerator SetToFalse(GameObject o)
    {
        yield return new WaitForSeconds(1);
        o.SetActive(false);
    }
    public void SwitchToTimer()
    {
        pomodoroTimer.gameObject.SetActive(true);
        LeanTween.moveX(twoPanels, -1080f, 0.3f);
        StartCoroutine(SetToFalse(pomodoroTimeSetter)); 
    }

    void Update()
    {
        
    }
    
}
