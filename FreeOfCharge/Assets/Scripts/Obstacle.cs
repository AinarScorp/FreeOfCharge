using System;
using System.Collections;
using System.Collections.Generic;
using Einar.Inputs;
using UnityEngine;

namespace Einar.Core
{
    public abstract class Obstacle : MonoBehaviour
    {
        [SerializeField] ParticleSystem _hitParticle;
        [SerializeField] LayerMask _layerMask;
        
        InputHandler _player;
        MeshRenderer _meshRenderer;
        Collider _collider;
        public InputHandler Player => _player;

        void Awake()
        {
            _collider = GetComponent<Collider>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        void TriggerObstaclePunishement()
        {
            ExecutePunishment();
            
            _collider.enabled = false;
            _meshRenderer.enabled = false;
        }


        void OnTriggerEnter(Collider other)
        {
            if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                _player = other.GetComponent<InputHandler>();
                if (_player == null) return;
                //Here I will put punishment for hitting an obstacle logic.
                if (_hitParticle != null)
                {
                    Instantiate(_hitParticle, transform.position, Quaternion.identity);
                }

                TriggerObstaclePunishement();
            }
        }

        protected virtual void ExecutePunishment()
        {
            Debug.Log("I'm empty");
        }
    }
}