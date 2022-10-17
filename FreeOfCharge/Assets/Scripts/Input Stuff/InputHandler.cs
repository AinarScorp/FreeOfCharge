using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using UnityEngine.InputSystem;


namespace Einar.Inputs
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



        public bool rightCorotineOn, leftCorOn, doubleOn, rightHeldOn, leftHeldOn;
        void Update()
        {
            rightCorotineOn = rightCoroutine != null;
            leftCorOn = leftCoroutine != null;
            doubleOn = doubleCoroutine != null;
        }
        

        void Awake()
        {
            //_mainInputs = new MainInputs();
            _playerInput = GetComponent<PlayerInput>();
            _inputActionAsset = _playerInput.actions;
            _inputActionMap = _inputActionAsset.FindActionMap("Main");

        }

        void OnEnable()
        {
            _inputActionMap.Enable();
            //_mainInputs.Enable();
        }

        void OnDisable()
        {
            _inputActionMap.Disable();

            //_mainInputs.Disable();
        }
        void Start()
        {
            _inputActionMap.FindAction("LeftButton").started += ctx => leftPressed =true;
            _inputActionMap.FindAction("LeftButton").performed += ctx => LeftPressed();
            _inputActionMap.FindAction("LeftButton").canceled += ctx =>
            {
                LeftButtonReleased?.Invoke();
                leftPressed = false;
            };
            
            // _mainInputs.Main.LeftButton.started += ctx => leftPressed =true;
            // _mainInputs.Main.LeftButton.performed += ctx => LeftPressed();
            // _mainInputs.Main.LeftButton.canceled += ctx =>
            // {
            //     LeftButtonReleased?.Invoke();
            //     leftPressed = false;
            // };
            _inputActionMap.FindAction("RightButton").started+= ctx => rightPressed =true;
            _inputActionMap.FindAction("RightButton").performed += ctx =>RightPressed();
            _inputActionMap.FindAction("RightButton").canceled += ctx =>
            {
                RightButtonReleased?.Invoke();

                rightPressed = false;
            };
            
            // _mainInputs.Main.RightButton.started += ctx => rightPressed =true;
            // _mainInputs.Main.RightButton.performed += ctx =>RightPressed();
            // _mainInputs.Main.RightButton.canceled += ctx =>
            // {
            //     RightButtonReleased?.Invoke();
            //
            //     rightPressed = false;
            // };
  
        }
    
        void RightPressed()
        {
            if (leftCoroutine != null || doubleCoroutine != null)
            {
                StartCoroutine(RightButtonHeld());
                return;
            }
            rightCoroutine = StartCoroutine(RightExecution());
            
        }
        void LeftPressed()
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

        IEnumerator RightButtonHeld()
        {
            while (leftCoroutine != null || doubleCoroutine != null)
            {
                if (!rightPressed) yield break;
                yield return null;
            }
            rightCoroutine = StartCoroutine(RightExecution());

        }
        IEnumerator LeftExecution()
        {

            yield return new WaitForSeconds(inputDelay);
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
        IEnumerator RightExecution()
        {
            yield return new WaitForSeconds(inputDelay);
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
            rightCoroutine = null;

        }
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
        void LeftActionPrint()
        {
            if (!leftPressed ) return;
            LeftButtonPressed?.Invoke();
        }
        void RightActionPrint()
        {
            if (!rightPressed) return;
            RightButtonPressed?.Invoke();
            
        }
        void DoubleActionPrint()
        {
            DoubleButtonPressed?.Invoke();
        }
    }
    
}
