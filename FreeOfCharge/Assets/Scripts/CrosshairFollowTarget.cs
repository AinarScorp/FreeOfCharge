using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using William;

namespace Einar.UI
{
    public class CrosshairFollowTarget : MonoBehaviour
    {
        Camera _mainCamera;
        ColorPicker _colorPicker;
        void Awake()
        {
            _mainCamera = Camera.main;
            _colorPicker = FindObjectOfType<ColorPicker>();
        }


        void Update()
        {
            if (!_colorPicker.DeliveryInRange) return;
            Vector3 screenPos = _mainCamera.WorldToScreenPoint(_colorPicker.DeliveryInRange.transform.position);
            this.transform.position = screenPos;
        }
    }
    
}
