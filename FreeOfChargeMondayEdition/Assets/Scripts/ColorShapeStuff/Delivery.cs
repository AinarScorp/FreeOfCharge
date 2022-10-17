using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelDesign.Road;
using Particles;

namespace DeliveryZoneInfo
{
    public enum DeliverableColor
    {
        Red,
        Blue,
        Yellow
    }

    public enum DeliverableShape
    {
        Emerald,
        Heart,
        Star
    }
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
        [SerializeField] ParticleSystem.MinMaxGradient[] _colorOverLifeTime;
        [SerializeField] ParticleSystem[] _ringParticles;
        [SerializeField] Material[] _colorMaterials;

        [SerializeField] MeshRenderer[] _shapes;
        [SerializeField] ParticleSystem deliveryCompleteParticle;
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

            _renderer = Instantiate(_shapes[(int)shape], this.transform);
            _renderer.material = _colorMaterials[(int)color];
        }
        public void InitializeDelivery(DeliverableColor color, DeliverableShape shape)
        {
            thisDeliveryInfo = new DeliveryInfo(color, shape);
            _renderer = Instantiate(_shapes[(int)shape], this.transform);
            _renderer.material = _colorMaterials[(int)color];
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
        }

        public void CompleteDelivery(DeliveryInfo deliveredInfo)
        {
            if (deliveryCompleteParticle != null)
            {
                RoadSimulation roadSimulation = FindObjectOfType<RoadSimulation>();

                ParticleSystem particleSystem = Instantiate(deliveryCompleteParticle, transform.position, deliveryCompleteParticle.transform.rotation, roadSimulation.transform);
                bool colorDelivered = thisDeliveryInfo.Color == deliveredInfo.Color;
                bool shapeDelivered = thisDeliveryInfo.Shape == deliveredInfo.Shape;

                particleSystem.GetComponent<ParticleCollector>().SetupParticle
                    (_renderer.GetComponent<MeshFilter>().mesh, _renderer.material, thisDeliveryInfo, colorDelivered, shapeDelivered);
            }

            this.gameObject.SetActive(false);
        }
    }

}