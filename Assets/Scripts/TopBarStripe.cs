using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TopBarStripe : MonoBehaviour
{
   [SerializeField] TMP_Text text;
   [SerializeField] FirebaseProgressSaver firebaseProgressSaver;
   //private Color _exposedTextColor = 

   void Awake()
   {
      firebaseProgressSaver.strikeChanged.AddListener(ChangeStripe);
   }
   void ChangeStripe()
   {
      Debug.Log("Stripe changed !");
      text.text = $"You working in <color=#FFA991>{firebaseProgressSaver.DaysInRow}<color=#FFFF> days in row !";
   }

}
