using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OrientationManager : MonoBehaviour
{
   [SerializeField] RectTransform rectTransform;

   private void Awake()
   {
      
   }

   private void Update()
   {
      Logger.Instance.Text = Input.deviceOrientation != DeviceOrientation.Portrait ? "Landscape" : "Portrait";
   }
   
}
