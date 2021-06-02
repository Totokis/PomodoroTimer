using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElasticButton : MonoBehaviour
{
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        if (_button != null)
        {
            _button.onClick.AddListener(Tween);
        }
    }

    void Tween()
    {
        LeanTween.cancel(gameObject);
        var gameObj = gameObject;
        gameObj.transform.localScale = Vector3.zero;

        LeanTween.scale(gameObj, Vector3.one, 0.5f).setEaseOutElastic();
    }
}
