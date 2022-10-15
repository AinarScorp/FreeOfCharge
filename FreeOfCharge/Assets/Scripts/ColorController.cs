using System;
using System.Collections;
using System.Collections.Generic;
using Einar.Inputs;
using Einar.UI;
using Unity.VisualScripting;
using UnityEngine;
using William;
namespace Einar.Core
{
    public class ColorController : MonoBehaviour
    {
        [SerializeField] DeliverableColor _deliverableColor;
        InputHandler _inputHandler;
        ColorPicker _colorPicker;
        bool _controlsInverted;
        void Awake()
        {
            _colorPicker = GetComponent<ColorPicker>();
            InputHandler[] inputHandlers = FindObjectsOfType<InputHandler>();
            foreach (var inputHandler in inputHandlers)
            {
                if (inputHandler.GetComponent<PlayerInfo>().Controls == PlayerControls.Shooter)
                {
                    _inputHandler = inputHandler;
                    break;
                }
            }
            //_inputHandler = GetComponent<InputHandler>();
        }

        void OnEnable()
        {
            //_inputHandler.LeftButtonPressed += SetColorIfInRange;
            SetupControls(false);
        }
        
        void OnDisable()
        {
            //_inputHandler.LeftButtonPressed -= SetColorIfInRange;
            DisableControls();
        
        }

        public void SetupControls(bool inverted)
        {
            _controlsInverted = inverted;
            _inputHandler.LeftButtonPressed += _controlsInverted? DiscardColor: Deliver;
            _inputHandler.RightButtonPressed += _controlsInverted? Deliver: DiscardColor;
        }
        
        public void DisableControls()
        {
            _inputHandler.LeftButtonPressed -= _controlsInverted? DiscardColor: Deliver;
            _inputHandler.RightButtonPressed -= _controlsInverted? Deliver: DiscardColor;
        }
        
        /// <summary>
        /// Delivers to a nearby delivery point.
        /// </summary>
        void Deliver()
        {
            _colorPicker.Deliver();
        }

        /// <summary>
        /// Discards the current color.
        /// </summary>
        void DiscardColor()
        {
            _colorPicker.DiscardDelivery();
        }
 
    }
    
}
