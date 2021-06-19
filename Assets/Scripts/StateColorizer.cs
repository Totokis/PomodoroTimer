using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StateColorizer:MonoBehaviour
{
    [SerializeField] Color backgroundPauseColor;
    [SerializeField] Color backgroundWorkColor;
    [SerializeField] Color buttonWorkColor;
    [SerializeField] Color buttonPauseColor;
    [SerializeField] Image image;

    [SerializeField] List<TMP_Text> buttons = new List<TMP_Text>();
    
    public void SetPauseColor()
    {
        image.color = backgroundPauseColor;
        foreach (TMP_Text button in buttons)
        {
            button.color = buttonPauseColor;
        }
    }
    public void SetWorkColor()
    {
        image.color = backgroundWorkColor;
        foreach (TMP_Text button in buttons)
        {
            button.color = buttonWorkColor;
        }
    }
}
