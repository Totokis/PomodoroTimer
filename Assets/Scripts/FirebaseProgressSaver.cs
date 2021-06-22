using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class FirebaseProgressSaver : MonoBehaviour
{ 
    [HideInInspector] public UnityEvent strikeChanged = new UnityEvent();
    [HideInInspector] public UnityEvent settingsLoaded = new UnityEvent();
    [SerializeField] PomodoroTimerViewModel pomodoroTimerViewModel;
    DateTime _lastWorkingDate;
    DatabaseReference _databaseReference; 
    FirebaseUser _user;
    int _daysInRow;
    public int DaysInRow => _daysInRow;
    string _settingsValue;
    public string SettingsValue => _settingsValue;
    
    IEnumerator Start()
    {
        _user = FirebaseAuth.DefaultInstance.CurrentUser;
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        pomodoroTimerViewModel.workDone.AddListener(SaveProgress);
        yield return LoadDaysFromFirebase();
        LoadProgress();
    }

    IEnumerator LoadDaysFromFirebase()
    {
        var dbTask = _databaseReference.Child("users").Child(_user.UserId).Child("DaysOfWork").GetValueAsync();
        yield return new WaitUntil(() => dbTask.IsCompleted);
        if (dbTask.IsCompleted)
        {
            if (dbTask.Result.Value != null)
            {
                _settingsValue = dbTask.Result.Value.ToString();
                settingsLoaded.Invoke();
            }
        }
        else
        {
            Debug.Log("Error loading saved days");
        }
        yield return new WaitForEndOfFrame();
    }

    async void LoadProgress()
    {
        LoadStrike();
        var lastSavedDate = await GetSavedDate();
        var todayDate = DateTime.Now;
        var daysInterval = new List<DayOfWeek>();
        //Get days from last working day
        foreach (DateTime day in EachCalendarDay(lastSavedDate,todayDate))
        {
            daysInterval.Add(day.DayOfWeek);
        }
        foreach (var day in daysInterval)
        {
            Debug.Log($"Interval: {day}");
            if (DayIsWorking(day))
            {
                 Debug.Log($"Working day: {day}");
                ResetStrike();
                break;
            }
        }
    }
    
    IEnumerable <DateTime> EachCalendarDay(DateTime startDate, DateTime endDate) {  
        //Method from https://www.c-sharpcorner.com/blogs/how-to-iterate-between-date-range
        //Modified from <= to <
        for (var date = startDate.Date; date.Date < endDate.Date; date = date.AddDays(1)) yield  
            return date;  
    }  
    
    private void LoadStrike()
    {
        StartCoroutine(LoadDaysStrikeFromFirebase());
    }
    private IEnumerator LoadDaysStrikeFromFirebase()
    {
        var dbTaskGetDays = _databaseReference.Child("users").Child(_user.UserId).Child("DayStrike").GetValueAsync();
        yield return new WaitUntil(() => dbTaskGetDays.IsCompleted);

        _daysInRow = dbTaskGetDays.Result.Value == null ? 0 : Int32.Parse(dbTaskGetDays.Result.Value.ToString());
        
        strikeChanged.Invoke();
    }
    private void ResetStrike()
    {
        _daysInRow = 0;
        strikeChanged.Invoke();
    }
    private bool DayIsWorking(DayOfWeek day)
    {
        var toggles = SettingsPanel.Instance.Toggles;
        Debug.Log($"Settings: {GetSettingsAsValue()}");
        int indexOfDay = (int)day==0?7:(int)day-1;
        return toggles[indexOfDay].isOn;
    }
    private async Task<DateTime> GetSavedDate()
    {
        await _databaseReference.Child("users").Child(_user.UserId).Child("LastWorkingDate").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("Error loading saved data");
            }
            else if (task.IsCompleted)
            {
                if (task.Result.Value == null)
                {
                    _lastWorkingDate = DateTime.Now.AddDays(-1);
                    return _lastWorkingDate;
                }
                Debug.Log("Last saved date: "+task.Result.Value.ToString());
                _lastWorkingDate = Convert.ToDateTime(task.Result.Value.ToString());
                return _lastWorkingDate;
            }
            _lastWorkingDate = DateTime.Now.AddDays(-1);
            return _lastWorkingDate;
        });
        return _lastWorkingDate;
    }
    private bool LastWorkingDateIsNotToday()
    {
        return _lastWorkingDate.Day != DateTime.Today.Day;
    }
    void SaveStrike()
    {
        StartCoroutine(SaveIntoFirebase());
    }
    private IEnumerator SaveIntoFirebase()
    {
        var saveDayStrikeTask = _databaseReference.Child("users").Child(_user.UserId).Child("DayStrike").SetValueAsync(_daysInRow);
        yield return new WaitUntil(() => saveDayStrikeTask.IsCompleted);
        var saveTodayDateTask = _databaseReference.Child("users").Child(_user.UserId).Child("LastWorkingDate").SetValueAsync(DateTime.Now.ToString("d"));
        yield return new WaitUntil(() => saveTodayDateTask.IsCompleted);
        yield return SaveDaysOfWork();

    }
    private string GetSettingsAsValue()
    {
        var toggles = SettingsPanel.Instance.Toggles;
        string value = "";
        foreach (var toggle in toggles)
        {
            value += toggle.isOn ? "1" : "0";
        }
        return value;
    }

    public void SaveWorkingDaysSettings()
    {
        StartCoroutine(SaveDaysOfWork());
    }
    private IEnumerator SaveDaysOfWork()
    {
        var saveDaysOfWork = _databaseReference.Child("users").Child(_user.UserId).Child("DaysOfWork").SetValueAsync(GetSettingsAsValue());
        yield return new WaitUntil(() => saveDaysOfWork.IsCompleted);
    }
    void SaveProgress()
    {
        if(LastWorkingDateIsNotToday())
            _daysInRow++;
        strikeChanged.Invoke();
        SaveStrike();
    }
}
