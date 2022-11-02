using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using UnityEngine.InputSystem;


namespace Inputs
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] bool leftPressed, rightPressed, bothPressed;
        [SerializeField] float inputDelay = 0.01f;
        Coroutine leftCoroutine, rightCoroutine, doubleCoroutine;
        
        //From the video
        InputActionAsset _inputActionAsset;
        InputActionMap _inputActionMap;
        PlayerInput _playerInput;


        public event Action LeftButtonPressed, RightButtonPressed, DoubleButtonPressed;
        public event Action LeftButtonReleased, RightButtonReleased, DoubleButtonReleased;

        MainInputs _mainInputs;


        

        void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _inputActionAsset = _playerInput.actions;
            _inputActionMap = _inputActionAsset.FindActionMap("Main");

        }

        void OnEnable()
        {
            _inputActionMap.FindAction("LeftButton").started += LeftBtnStarted;
            _inputActionMap.FindAction("LeftButton").performed += LeftPressed;
            _inputActionMap.FindAction("LeftButton").canceled += LeftBtnReleased;

            _inputActionMap.FindAction("RightButton").started+= RightBtnStarted;
            _inputActionMap.FindAction("RightButton").performed +=RightPressed;
            _inputActionMap.FindAction("RightButton").canceled += RightBtnReleased;
            _inputActionMap.Enable();

        }


        void OnDisable()
        {
            _inputActionMap.FindAction("LeftButton").started -= LeftBtnStarted;
            _inputActionMap.FindAction("LeftButton").performed -= LeftPressed;
            _inputActionMap.FindAction("LeftButton").canceled -= LeftBtnReleased;
            
            _inputActionMap.FindAction("RightButton").started-= RightBtnStarted;
            _inputActionMap.FindAction("RightButton").performed -= RightPressed;
            _inputActionMap.FindAction("RightButton").canceled -= RightBtnReleased;
            _inputActionMap.Disable();
            StopAllCoroutines();

        }

        #region LeftAction
        
        void LeftBtnStarted(InputAction.CallbackContext ctx)
        {
            leftPressed = true;
        }
        
        void LeftBtnReleased(InputAction.CallbackContext ctx)
        {
            LeftButtonReleased?.Invoke();
            leftPressed = false;
        }
        void LeftPressed(InputAction.CallbackContext ctx)
        {
            if (rightCoroutine != null || doubleCoroutine != null)
            {
                StartCoroutine(LeftButtonHeld());
                return;
            }

            leftCoroutine = StartCoroutine(LeftExecution());
        }
        IEnumerator LeftButtonHeld()
        {
            while (rightCoroutine != null || doubleCoroutine != null)
            {
                if (!leftPressed) yield break;
                yield return null;
            }
            leftCoroutine = StartCoroutine(LeftExecution());
        }
        IEnumerator LeftExecution()
        {
            yield return new WaitForSecondsRealtime(inputDelay);
            if (rightPressed)
            {
                doubleCoroutine =StartCoroutine(DoubleCoroutine());
                yield break;
            }
            LeftActionPrint();
            while (leftPressed)
            {
                yield return null;
            }
            leftCoroutine = null;
        }
        void LeftActionPrint()
        {
            if (!leftPressed ) return;
            LeftButtonPressed?.Invoke();
        }

        #endregion
        
        #region RightAction

        void RightBtnStarted(InputAction.CallbackContext ctx)
        {
            rightPressed = true;
        }
        void RightBtnReleased(InputAction.CallbackContext ctx)
        {
            RightButtonReleased?.Invoke();
            rightPressed = false;
        }
        void RightPressed(InputAction.CallbackContext ctx)
        {
            if (leftCoroutine != null || doubleCoroutine != null)
            {
                StartCoroutine(RightButtonHeld());
                return;
            }
            rightCoroutine = StartCoroutine(RightExecution());
            
        }
        IEnumerator RightButtonHeld()
        {
            while (leftCoroutine != null || doubleCoroutine != null)
            {
                if (!rightPressed) yield break;
                yield return null;
            }
            rightCoroutine = StartCoroutine(RightExecution());

        }
        IEnumerator RightExecution()
        {
            yield return new WaitForSecondsRealtime(inputDelay);

            if (leftPressed)
            {
                doubleCoroutine =StartCoroutine(DoubleCoroutine());
                yield break;
            }
            RightActionPrint();
            while (rightPressed)
            {
                yield return null;
            }
            DoubleButtonReleased?.Invoke();
            rightCoroutine = null;

        }

        void RightActionPrint()
        {
            if (!rightPressed) return;
            RightButtonPressed?.Invoke();
            
        }


        #endregion
        
        #region DoubleAction

        IEnumerator DoubleCoroutine()
        {
            rightCoroutine = null;
            leftCoroutine = null;
            if (doubleCoroutine !=null) yield break;
            DoubleActionPrint();
            while (rightPressed && leftPressed)
            {
                yield return null;
            }
            doubleCoroutine = null;
        }
        void DoubleActionPrint()
        {
            DoubleButtonPressed?.Invoke();
        }

        #endregion














    }
    
}
