using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelDesign.Road
{
    public class RoadSimulation : MonoBehaviour
    {

        public static event Action<float> OnSpeedChange;

        [Tooltip("Use this to select the direction every child in this gameobject is gonna be moving")]
        [SerializeField] Vector2 _moveDirection = Vector2.down;
        [SerializeField] float _simulationSpeed = 10.0f;
        float _speedMultiplier = 0f;
        [SerializeField] float _maxSpeed;
        [SerializeField] float _minSpeed;
        [SerializeField] float _acceleration;



        void Update()
        {
            ChangeSpeed(Time.fixedDeltaTime * _acceleration); 
            SimulateRoad();
        }

        void SimulateRoad()
        {
            Vector3 direction = new Vector3(_moveDirection.x, 0, _moveDirection.y).normalized;
            transform.position += direction * (_simulationSpeed * _speedMultiplier * Time.deltaTime);
        }

        public void ChangeSpeed(float increase = .1f)
        {
            _speedMultiplier = Mathf.Clamp(_speedMultiplier + increase, _minSpeed, _maxSpeed);
            OnSpeedChange?.Invoke(_speedMultiplier);
        }

        public void ResetSpeed()
        {
            _speedMultiplier = _minSpeed;
            OnSpeedChange?.Invoke(_speedMultiplier);
        }
    }
    
}
