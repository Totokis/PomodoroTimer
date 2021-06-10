using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseButton : MonoBehaviour
{
    [SerializeField] bool onClickStopAnimating = false;
    [SerializeField] float scale = 1.5f;
    [SerializeField] float time = 0.3f;
    RectTransform _rect;
    Button _button;
    bool _animate = true;
    void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener((() => {
            if (onClickStopAnimating)
                _animate = false;
        }));
    }
    void OnEnable()
    {
       if(_animate)
           PulseAnimation();
    }

    void PulseAnimation()
    {
        LeanTween.scale(_rect, new Vector2(scale, scale), time).setLoopPingPong(3).setOnComplete(() => {
            if (_animate)
            {
                PulseAnimation();
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 0);
            }
        });
    }
    
    
}
