using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Shooter;
using DeliveryZoneInfo;
using Ui.DeliverInfo;

namespace Particles
{

    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleCollector : MonoBehaviour
    {
        [SerializeField] ChargePointUi chargePoint;

        [SerializeField] float chargeAmountPerParticle = .1f;
        [SerializeField] Renderer _renderer;

        [Tooltip("CompleteReward means both color and shape were correct on top of partial rewards")] 
        [SerializeField] int completeReward = 5;
        [SerializeField] int partialReward = 5;

        List<ParticleSystem.Particle> _particles = new List<ParticleSystem.Particle>();
        
        ColorPicker player;
        
        DeliverableColor delColor;
        DeliverableShape delShape;
        //bool colorDelivered, shapeDelivered;

        ParticleSystem _particleSystem;
        CountDownImage _chargeUI;

        //CountDownImage colorChargeUI, shapeChargeUI;
        void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            player = FindObjectOfType<ColorPicker>();
            _renderer = GetComponent<Renderer>();

        }

        void Start()
        {
            _particleSystem.trigger.AddCollider(player.GetComponent<CharacterController>());
            _chargeUI = FindObjectOfType<CountDownImage>();
            // if (countDownImage == null)
            // {
            //     return;
            // }
            // foreach (var countDownImage in countDownImages)
            // {
            //     if (countDownImage.DeliveryContainerColor != null)
            //     {
            //         colorChargeUI = countDownImage;
            //     }
            //
            //     if (countDownImage.DeliveryContainerShape !=null)
            //     {
            //         shapeChargeUI = countDownImage;
            //     }
            // }
        }



        void OnParticleTrigger()
        {
            int triggeredParticles = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
            for (int i = 0; i < triggeredParticles; i++)
            {
                ParticleSystem.Particle particle = _particles[i];
                particle.remainingLifetime = 0;
                ColorPicker colorPicker = _particleSystem.trigger.GetCollider(0).GetComponent<ColorPicker>();
                if (colorPicker != null)
                    StartCoroutine(DragPointToChargeUI(colorPicker, _chargeUI.ImageTransform));
                
                // if (colorDelivered)
                // {
                //
                // }
                //
                // if (shapeDelivered)
                // {                    
                //     if (colorPicker!=null) 
                //         StartCoroutine(DragPointToChargeUI(colorPicker, shapeChargeUI.ImageTransform, false));
                // }

                //print("collected particle  " + _particleSystem.trigger.GetCollider(0).name);
                _particles[i] = particle;
                FMODUnity.RuntimeManager.PlayOneShot ("event:/Delivery Zones/Pickup Particles/Pickup Particles");


            }

            _particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
        }

        IEnumerator DragPointToChargeUI(ColorPicker colorPicker, RectTransform chargePos)
        {
            if (colorPicker == null) yield break;
            if (chargePos == null)yield break;

            Vector3 chargeUIstartPos = Camera.main.WorldToScreenPoint(player.transform.position);

            Canvas canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
            ChargePointUi createdPoint=  Instantiate(chargePoint,canvas.transform);
            yield return createdPoint.DeliverPoint(chargeUIstartPos,chargePos);
            colorPicker.ModifyCharge(chargeAmountPerParticle, true);
            // if (isColor)
            // {
            //     colorPicker.ModifyCharge(delColor, chargeAmountPerParticle, true);
            //
            // }
            // else
            // {
            //     colorPicker.ModifyCharge(delShape, chargeAmountPerParticle, true);
            //
            // }
            
        }
        public void SetupParticle(Mesh shapeMesh, Material colorMaterial, DeliveryInfo info, bool colorDelivered, bool shapeDelivered)
        {
            ParticleSystemRenderer particleSystemRenderer = (ParticleSystemRenderer)_renderer;
            //Get information
            particleSystemRenderer.mesh = shapeMesh;
            particleSystemRenderer.material = colorMaterial;
            delColor = info.Color;
            delShape = info.Shape;

            //Setup burst
            ParticleSystem.Burst burst = _particleSystem.emission.GetBurst(0);
            burst.count = GetBurstCount(colorDelivered,shapeDelivered);
            _particleSystem.emission.SetBurst(0, burst);
            
            
            _particleSystem.Play();
        }

        int GetBurstCount(bool colorDelivered, bool shapeDelivered)
        {
            int burstCount = 0;
            if (colorDelivered) burstCount += partialReward;
            if (shapeDelivered) burstCount += partialReward;
            if (colorDelivered && shapeDelivered) burstCount += completeReward;
            return burstCount;
        }
    }
}