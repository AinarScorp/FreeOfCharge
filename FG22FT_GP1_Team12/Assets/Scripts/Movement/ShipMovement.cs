using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;
using Unity.VisualScripting;

//TODO make handleMovement method prettier
namespace Player.Driver
{
    [RequireComponent(typeof(CharacterController))]
    public class ShipMovement : MonoBehaviour
    {
        [Header("Speed/Steering")] 
        [SerializeField]
        float _maxSpeed = 20.0f;
        [SerializeField] float _acceleration = 1.0f;

        [SerializeField] float _deceleration = 1.0f;

        //gravity

        [Header("Gravity stuff")] // 
        [SerializeField] float _gravity = -15.0f;
        [SerializeField] float _heavierGravity = -30.0f;
        [SerializeField] float _gravityMultiplier = 2;
        [SerializeField] float groundedRadius = 0.5f;
        [SerializeField] LayerMask _groundLayers;
        float _verticalVelocity;


        [Header("Jumping Settings")] // 
        [SerializeField] float _heightToReach = 5f;
        [SerializeField] float _jumpCooldown = 0.5f;
        float _jumpCooldownTimer;

        InputHandler _inputHandler;

        CharacterController _characterController;

        //inputs
        [Header("Don't touch for now, only for seeing stuff")] // 

        [SerializeField] float _currentSpeed;
        bool jump, goingLeft, goingRight;

        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            InputHandler[] inputHandlers = FindObjectsOfType<InputHandler>();
            foreach (var inputHandler in inputHandlers)
            {
                if (inputHandler.GetComponent<PlayerInfo>().Controls == PlayerControls.Mover)
                {
                    _inputHandler = inputHandler;
                    break;
                }
            }
            


        }
        void AssignControls()
        {
            SetupDoubleAction();
            SetupLeftAction();
            SetupRightAction();
        }

        public void DeactiveContols()
        {
            
        }

        void SetupRightAction()
        {
            _inputHandler.RightButtonPressed += () => { goingRight = true; };
            _inputHandler.RightButtonReleased += () =>
            {
                jump = false;
                goingRight = false;
            };
        }

        void SetupLeftAction()
        {
            _inputHandler.LeftButtonPressed += () => { goingLeft = true; };
            _inputHandler.LeftButtonReleased += () =>
            {
                jump = false;
                goingLeft = false;
            };
        }

        void SetupDoubleAction()
        {
            _inputHandler.DoubleButtonPressed += () =>
            {
                jump = true;
                goingRight = false;
                goingLeft = false;
            };
        }

        void Start()
        {
            _currentSpeed = 0;
            AssignControls();

        }


        void Update()
        {
            HandleMovement();
            JumpAction();
            ChangeVelocityByGravity();
        }


        void HandleMovement()
        {
            float xInput = goingLeft ? -1 : goingRight ? 1 : 0;
            // if (goingLeft || goingRight)
            // {
            //     _currentSpeed += _acceleration * Time.deltaTime;
            //
            //
            //     if (Mathf.Abs(_currentSpeed) > Mathf.Abs(_maxSpeed))
            //     {
            //         if (_currentSpeed < 0)
            //         {
            //             _currentSpeed = -_maxSpeed;
            //         }
            //
            //         if (_currentSpeed > 0)
            //         {
            //             _currentSpeed = _maxSpeed;
            //         }
            //     }
            // }
            if (goingLeft)
            {
                _currentSpeed -= _acceleration * Time.deltaTime;
                if (_currentSpeed<-_maxSpeed)
                {
                    _currentSpeed = -_maxSpeed;
                }
            }
            if (goingRight)
            {
                _currentSpeed += _acceleration* Time.deltaTime;
                if (_currentSpeed>_maxSpeed)
                {
                    _currentSpeed = _maxSpeed;
            
                }
            }

            if (!goingLeft && !goingRight)
            {
                if (_currentSpeed < 0.1f)
                {
                    _currentSpeed += _deceleration * Time.deltaTime;
                }

                else if (_currentSpeed > 0.1f)
                {
                    _currentSpeed -= _deceleration * Time.deltaTime;
                }
                else
                {
                    _currentSpeed = 0;
                }
            }

            Vector3 xMotion = new Vector3(_currentSpeed, 0, 0);

            //Vector3 xMotion = new Vector3(xInput, 0, 0);
            Vector3 yMotion = new Vector3 { x = 0, y = _verticalVelocity, z = 0 };
            _characterController.Move(xMotion * Time.deltaTime + yMotion * Time.deltaTime);
        }


        void ChangeVelocityByGravity()
        {
            float pushDownMultiplier = _verticalVelocity < 0.0f ? _gravityMultiplier : 1;
            float gravityToUse = _gravity;
            if (_verticalVelocity < 0.0f)
            {
                if (IsGrounded())
                {
                    _verticalVelocity = -2.0f;
                    pushDownMultiplier = 1f;
                }

                gravityToUse *= pushDownMultiplier;
            }
            else if (!IsGrounded() && _inputHandler.enabled && !jump)
            {
                gravityToUse = _heavierGravity;
            }


            _verticalVelocity += gravityToUse * Time.deltaTime;
        }

        void ApplyJumpVelocity(float heightToReach)
        {
            _verticalVelocity = Mathf.Sqrt(_gravity * heightToReach * -2f);
        }

        bool IsGrounded()
        {
            return Physics.CheckSphere(transform.position, groundedRadius, _groundLayers);
        }

        void JumpAction()
        {
            if (_jumpCooldownTimer >= 0.0f) _jumpCooldownTimer -= Time.deltaTime;
            if (!jump) return;
            if (!IsGrounded() || _jumpCooldownTimer >= 0.0f) return;
            ApplyJumpVelocity(_heightToReach);
            _jumpCooldownTimer = _jumpCooldown;
            jump = false;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, groundedRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + _heightToReach, transform.position.z));
        }
    }
}