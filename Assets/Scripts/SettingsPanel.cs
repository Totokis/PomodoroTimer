using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    static public SettingsPanel Instance { get;  set; }
    [SerializeField] Button switchToSettingsSetterButton;
    [SerializeField] Button switchToSettingsTimerButton;
    [SerializeField] Button exitButton;
    [SerializeField] List<Toggle> toggles;
    [SerializeField] FirebaseProgressSaver firebaseProgressSaver;
    string _settingsValue;
    public List<Toggle> Toggles => toggles;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            switchToSettingsSetterButton.onClick.AddListener(SwitchToSettings);
            switchToSettingsTimerButton.onClick.AddListener(SwitchToSettings);
            exitButton.onClick.AddListener(SwitchToPanels);
            firebaseProgressSaver.settingsLoaded.AddListener(() => {
                Debug.Log("Setting LOADED");
                _settingsValue = firebaseProgressSaver.SettingsValue;
                LoadSettings();
            });
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        
    }
    void LoadSettings()
    {
        for (int i = 0; i < _settingsValue.Length; i++)
        {
            toggles[i].isOn = _settingsValue[i] == '1';
        }
    }

    void SwitchToPanels()
    {
        LeanTween.moveY(gameObject, 1920+960, 0.3f);
    }
    void SwitchToSettings()
    {
        LeanTween.moveY(gameObject, 1920, 0.3f);
    }
}
