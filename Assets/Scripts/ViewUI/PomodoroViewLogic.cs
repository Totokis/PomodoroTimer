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
        startButton.onClick.AddListener(() => {
            LeanTween.moveX(twoPanels, 0f, 0.3f);
        });
        endButton.onClick.AddListener(() => {
            LeanTween.moveX(twoPanels, 1080f, 0.3f);
        });
        
    }
    
    void Update()
    {
        
    }
    
}
