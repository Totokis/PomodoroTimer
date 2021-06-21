using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StateColorizer:MonoBehaviour
{
    [SerializeField]  Color backgroundPauseColor;
    [SerializeField]  Color backgroundWorkColor;
    [SerializeField]  Color buttonWorkColor;
    [SerializeField]  Color buttonPauseColor;
    [SerializeField]  List<Image> images;

    [SerializeField]  List<TMP_Text> buttons = new List<TMP_Text>();
    
    public void SetPauseColor()
    {
        
        foreach (Image image in images)
        {
            image.color = backgroundPauseColor;
        }
        foreach (TMP_Text button in buttons)
        {
            button.color = buttonPauseColor;
        }
    }
    public void SetWorkColor()
    {
        foreach (Image image in images)
        {
            image.color = backgroundWorkColor;
        }
        foreach (TMP_Text button in buttons)
        {
            button.color = buttonWorkColor;
        }
    }
}
