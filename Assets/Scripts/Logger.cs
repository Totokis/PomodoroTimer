using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
   static public Logger Instance { get; private set; }
   TMP_Text _text;
   public String Text { get => _text.text; set => _text.text = value; }
   
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
   
}
