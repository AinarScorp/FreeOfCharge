using System;
using System.Collections;
using System.Collections.Generic;
using Einar.Inputs;
using UnityEngine;

namespace Einar.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class ShipMovement : MonoBehaviour
    {
        //gravity
        [Header("gravity")] // 
        float _verticalVelocity;

        [SerializeField] float _gravity = -15.0f;
        [SerializeField] float _heavierGravity = -30.0f;
        [SerializeField] float _gravityMultiplier = 2;
        [SerializeField] float groundedRadius = 0.5f;
        [SerializeField] LayerMask _groundLayers;

        [Header("speed")] // 
        [SerializeField]
        float _maxSpeed = 20.0f;

        [SerializeField] float _minSpeed = 20.0f;

        [Header("Jumping")] // 
        float _jumpCooldownTimer;

        [SerializeField] float _heightToReach = 5f;
        [SerializeField] float _jumpCooldown = 0.5f;


        InputHandler _inputHandler;
        CharacterController _characterController;
        //inputs
        float _currentSpeed;
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
            //_inputHandler = GetComponent<InputHandler>();
            _inputHandler.LeftButtonPressed += () => { goingLeft = true; };
            _inputHandler.RightButtonPressed += () => { goingRight = true; };
            _inputHandler.DoubleButtonPressed += () =>
            {
                jump = true;
                goingRight = false;
                goingLeft = false;
            };
            _inputHandler.LeftButtonReleased += () =>
            {
                jump = false;
                goingLeft = false;
            };
            _inputHandler.RightButtonReleased += () =>
            {
                jump = false;
                goingRight = false;
            };
        }

        void Start()
        {
            _currentSpeed = _minSpeed;
        }

        bool printedOnce;

        void Update()
        {
            HandleMovement();
            JumpAction();
            ChangeVelocityByGravity();
        }


        public void ResetSpeed()
        {
            _currentSpeed = _minSpeed;
        }

        void HandleMovement()
        {
            float xInput = goingLeft ? -1 : goingRight ? 1 : 0;

            Vector3 xMotion = new Vector3(xInput, 0, 0);
            Vector3 yMotion = new Vector3 { x = 0, y = _verticalVelocity, z = 0 };
            _characterController.Move(xMotion * _currentSpeed * Time.deltaTime + yMotion * Time.deltaTime);
        }

        bool _controlsInverted;
        public void SetInversion(bool setTo) => _controlsInverted = setTo;


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