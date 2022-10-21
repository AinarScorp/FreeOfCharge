using Player.Shooter;
using System;
using UnityEngine;

namespace Tutorial
{
    public class ColorPickerTutorial : ColorPicker
    {
        public static event Action OnShootDelivery;
        [HideInInspector] public int ColorsActive;
        [HideInInspector] public int ShapesActive;

        [HideInInspector] public bool ColorLocked = false;
        [HideInInspector] public bool ShapeLocked = false;
        [HideInInspector] public bool CanShoot = true;

        public override void SelectNextDelColor()
        {
            if (ColorLocked) return;

            int nextIndex = _deliverableColors.IndexOf(_currentDeliveryColor);
            nextIndex++;
            nextIndex %= ColorsActive;

            _currentDeliveryColor = _deliverableColors[nextIndex];
            InvokeNewColor();
            PlayColorSelectSound();
        }

        public override void SelectNextDelShape()
        {
            if (ShapeLocked) return;

            int nextIndex = _deliverableShapes.IndexOf(_currentDeliveryShape);
            nextIndex++;
            nextIndex %= ShapesActive;

            _currentDeliveryShape = _deliverableShapes[nextIndex];
            InvokeNewShape();
            PlayShapeSelectSound();
        }

        public override void ShootDelivery()
        {
            if (!CanShoot) return;

            base.ShootDelivery();
            OnShootDelivery?.Invoke();
        }
    }
}
