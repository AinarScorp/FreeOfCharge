using System;
using System.Collections;
using System.Collections.Generic;
using Animations;
using Inputs;
using UnityEngine;

namespace LevelDesign.Obstacles
{
    public abstract class Obstacle : MonoBehaviour
    {
        [SerializeField] ParticleSystem _hitParticle;
        [SerializeField] LayerMask _layerMask;
        [SerializeField] float _destoryDelay = 2.0f;
        MeshRenderer _meshRenderer;
        Collider _collider;
        CharacterController _player;

        public CharacterController Player => _player;


        void Awake()
        {
            _collider = GetComponentInChildren<Collider>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }



        void OnTriggerEnter(Collider other)
        {
            if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                _player = other.GetComponent<CharacterController>();
                if (_player == null) return;
                TriggerPlayerHitAnimation();
                if (_hitParticle != null)
                {
                    Instantiate(_hitParticle, transform.position, _hitParticle.transform.rotation);
                }

                //Here I will put punishment for hitting an obstacle logic.
                TriggerObstaclePunishement();
            }
        }

        void TriggerPlayerHitAnimation()
        {
            AnimationController playerAnimations = _player.GetComponent<AnimationController>();
            if (playerAnimations == null) return;

            playerAnimations.TriggerGetHitAnimation();
        }

        void TriggerObstaclePunishement()
        {
            ExecutePunishment();

            VanishDestroy();
        }
        protected virtual void ExecutePunishment()
        {
            Debug.Log("I'm empty");
        }
        void VanishDestroy()
        {
            _collider.enabled = false;
            _meshRenderer.enabled = false;
            Destroy(this.gameObject, _destoryDelay);
        }


    }
}