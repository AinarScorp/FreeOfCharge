using System;
using System.Collections;
using System.Collections.Generic;
using Player.Shooter;
using UnityEngine;
using DeliveryZoneInfo;


namespace Animations
{
    
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] Animator[] _animators;
        [SerializeField] ColorPicker _colorPicker;
        [SerializeField] string shapeSwap = "ShapeSwap", _getHit = "GetHit", _shoot = "Shoot";
        void Awake()
        {
            if (_animators == null)
            {
                _animators = GetComponentsInChildren<Animator>();
            }

            if (_colorPicker ==null)
            {
                _colorPicker = GetComponent<ColorPicker>();
            }
            _colorPicker.NewColorSelected += TriggerChangeColor;

            _colorPicker.NewShapeSelected += TriggerChangeShape;

            _colorPicker.HasShot += TriggerShootAnimation;
        }

        void TriggerChangeShape(DeliveryContainer<DeliverableShape> shape)
        {
            foreach (var animator in _animators)
            {
                animator.SetTrigger(shapeSwap);
            }
        }
        void TriggerChangeColor(DeliveryContainer<DeliverableColor> color)
        {
            foreach (var animator in _animators)
            {
                animator.SetTrigger(shapeSwap);
            }
        }

        void TriggerShootAnimation()
        {
            foreach (var animator in _animators)
            {
                animator.SetTrigger(_shoot);
            }
        }

        public void TriggerGetHitAnimation()
        {
            foreach (var animator in _animators)
            {
                animator.SetTrigger(_getHit);
            }
        }
        
    }
}
