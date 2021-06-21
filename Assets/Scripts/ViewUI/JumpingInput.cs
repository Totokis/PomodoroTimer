using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JumpingInput : MonoBehaviour
{
     void Awake()
    {
        GetComponent<TMP_InputField>().onSelect.AddListener((_)=>{LeanTween.cancel(gameObject);});
    }
     void OnEnable()
    {
        LeanTween.moveLocal(gameObject, Vector3.up*10, 0.3f).setLoopPingPong();
    }
}
