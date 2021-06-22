using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
   static public Logger Instance { get;  set; }

    TMP_Text _text;
    private string voidString;
   public String Text { get => _text.text; set => voidString = value; }

    void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         DontDestroyOnLoad(this);
         _text = GetComponent<TMP_Text>();
      }
      else
      {
         Destroy(this);
      }
   }

   public void SetText(string text)
   {
      Text = text;
   }

}
