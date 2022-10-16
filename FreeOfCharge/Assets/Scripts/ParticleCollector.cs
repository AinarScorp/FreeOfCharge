using System;
using System.Collections;
using System.Collections.Generic;
using Einar.Movement;
using UnityEngine;
using William;

public class ParticleCollector : MonoBehaviour
{
    ParticleSystem _particleSystem;
    List<ParticleSystem.Particle> _particles = new List<ParticleSystem.Particle>();
    ShipMovement player;
    [SerializeField]Renderer _renderer;
    DeliverableColor delColor;
    DeliverableShape delShape;
    bool colorDelivered, shapeDelivered;
    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        player = FindObjectOfType<ShipMovement>();
        _renderer = GetComponent<Renderer>();
        
    }

    void Start()
    {
        
        _particleSystem.trigger.AddCollider(player.GetComponent<CharacterController>());
        
    }



    void OnParticleTrigger()
    {
        int triggeredParticles = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
        for (int i = 0; i < triggeredParticles; i++)
        {
            ParticleSystem.Particle particle = _particles[i];
            particle.remainingLifetime = 0;
            ColorPicker colorPicker = _particleSystem.trigger.GetCollider(0).GetComponent<ColorPicker>();
            if (colorDelivered)
            {
                colorPicker.AddCharge(delColor);
            }

            if (shapeDelivered)
            {
                colorPicker.AddCharge(delShape);
            }
            //print("collected particle  " + _particleSystem.trigger.GetCollider(0).name);
            _particles[i] = particle;
            

        }
        _particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
    }

    public void SetupParticle(Mesh shapeMesh, Material colorMaterial, DeliveryInfo info, bool colorDelivered, bool shapeDelivered)
    {
        ParticleSystemRenderer particleSystemRenderer = (ParticleSystemRenderer)_renderer;
        particleSystemRenderer.mesh = shapeMesh;
        particleSystemRenderer.material = colorMaterial;
        delColor = info.Color;
        delShape = info.Shape;
        this.colorDelivered = colorDelivered;
        this.shapeDelivered = shapeDelivered;
        int burstCount = 0;
        if (colorDelivered)
        {
            burstCount += 5;
        }
        
        if (shapeDelivered)
        {
            burstCount += 5;
        }

        if (colorDelivered && shapeDelivered)
        {
            burstCount += 10;

        }
        ParticleSystem.Burst burst = _particleSystem.emission.GetBurst(0);
        burst.count = burstCount;
        _particleSystem.emission.SetBurst(0, burst);
        _particleSystem.Play();
    }

}
