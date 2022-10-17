using System;
using System.Collections;
using System.Collections.Generic;
using Einar.Core;
using UnityEngine;

namespace William
{
    public struct DeliveryInfo
    {
        public DeliverableColor Color { get; private set; }
        public DeliverableShape Shape { get; private set; }

        public DeliveryInfo(DeliverableColor color, DeliverableShape shape)
        {
            Color = color;
            Shape = shape;
        }
    }

    public class Delivery : MonoBehaviour
    {
        public static event Action<bool, bool> OnCompletedDelivery;

        [Tooltip("Red, Blue, Yellow")] [SerializeField]
        ParticleSystem.MinMaxGradient[] _colorOverLifeTime;

        [SerializeField] ParticleSystem[] _ringParticles;

        [Tooltip("Emerald, Heart, Star")] [SerializeField]
        Material[] _colorMaterials;

        [SerializeField] MeshRenderer[] _shapes;
        [SerializeField] ParticleSystem YeyParticle;
        [HideInInspector] public bool DeliveryStarted = false;
        [SerializeField] bool isPlayerReference = false;
        Renderer _renderer;
        MeshFilter _meshFilter;
        DeliveryInfo thisDeliveryInfo;

        public Material[] ColorMaterials => _colorMaterials;

        public MeshRenderer[] Shapes => _shapes;

        void Start()
        {
            if (!isPlayerReference)
            {
                InitializeDelivery((DeliverableColor)UnityEngine.Random.Range(0, 3), (DeliverableShape)UnityEngine.Random.Range(0, 3));
            }
        }

        public void DisplayDeliveryAboveHead(DeliverableColor color, DeliverableShape shape)
        {
            //thisDeliveryInfo = new DeliveryInfo(color, shape);
            if (_renderer != null)
            {
                Destroy(_renderer.gameObject);
            }
            //
            // if (color == null || shape -- null)
            // {
            //     
            // }
            _renderer = Instantiate(_shapes[(int)shape], this.transform);
            _renderer.material = _colorMaterials[(int)color];
        }

        public void InitializeDelivery(DeliverableColor color, DeliverableShape shape)
        {
            thisDeliveryInfo = new DeliveryInfo(color, shape);
            _renderer = Instantiate(_shapes[(int)shape], this.transform);
            _renderer.material = _colorMaterials[(int)color];
            //_meshFilter.mesh = _meshs[(int)shape].GetComponent<MeshFilter>().sharedMesh;

            //_meshFilter.mesh = _shapes[(int)shape].mesh;
            SetRingParticleColor(color);
        }

        void SetRingParticleColor(DeliverableColor color)
        {
            if (_ringParticles == null) return;

            foreach (var ringParticle in _ringParticles)
            {
                ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = ringParticle.colorOverLifetime;
                colorOverLifetimeModule.color = _colorOverLifeTime[(int)color];
            }

            ToggleParticle(false);
        }

        /// <summary>
        /// Starts the delivery.
        /// </summary>
        public void StartDelivery()
        {
            DeliveryStarted = true;
        }

        /// <summary>
        /// Completes a delivery.
        /// </summary>
        public void CompleteDelivery(bool correctColor, bool correctShape)
        {
            OnCompletedDelivery?.Invoke(correctColor, correctShape);
            gameObject.SetActive(false);
        }

        public void CompleteDelivery(DeliveryInfo deliveredInfo)
        {
            if (YeyParticle != null)
            {
                RoadSimulation roadSimulation = FindObjectOfType<RoadSimulation>();

                ParticleSystem particleSystem = Instantiate(YeyParticle, transform.position, YeyParticle.transform.rotation, roadSimulation.transform);
                bool colorDelivered = thisDeliveryInfo.Color == deliveredInfo.Color;
                bool shapeDelivered = thisDeliveryInfo.Shape == deliveredInfo.Shape;

                particleSystem.GetComponent<ParticleCollector>().SetupParticle
                    (_renderer.GetComponent<MeshFilter>().mesh, _renderer.material, thisDeliveryInfo, colorDelivered, shapeDelivered);
            }

            this.gameObject.SetActive(false);
        }

        public void ToggleParticle(bool setTo)
        {
            return;
            foreach (var ringParticle in _ringParticles)
            {
                ringParticle.gameObject.SetActive(setTo);
            }
        }
    }
}