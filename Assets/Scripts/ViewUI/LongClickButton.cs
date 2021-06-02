using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ViewUI
{
    public class LongClickButton : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
    {
        [SerializeField] float requiredHoldTime;
        public UnityEvent onLongClick = new UnityEvent();
        public UnityEvent onClick = new UnityEvent();

        bool _pointerDown;
        float _pointerDownTimer;

        void Awake()
        {
            GetComponent<Button>()?.onClick.AddListener(()=>onClick.Invoke());
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDown = true;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            _pointerDown = false;
            Reset();
        }
        void Reset()
        {
            _pointerDownTimer = 0;
        }

        void Update()
        {

            if (_pointerDown)
            {
                _pointerDownTimer += Time.deltaTime;
                if (_pointerDownTimer > requiredHoldTime)
                {
                    if (onLongClick != null)
                    {
                        onLongClick.Invoke();
                    }
                    Reset();
                }
                else
                {
                    
                }
            }
        }
    }
}
