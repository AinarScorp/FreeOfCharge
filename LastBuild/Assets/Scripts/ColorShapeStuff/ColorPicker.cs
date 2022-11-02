using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using DeliveryZoneInfo;
using TMPro;

namespace Player.Shooter
{
    public class ColorPicker : MonoBehaviour
    {
        public event Action<DeliveryContainer<DeliverableColor>> NewColorSelected;
        public event Action<DeliveryContainer<DeliverableShape>> NewShapeSelected;
        public event Action HasShot;
        public event Action<bool> ChargeModified;

        [Header("Ammo Setup")]
        [SerializeField] float _maxCharges = 5;

        [SerializeField] float _shootChargeSubtraction = -1;


        [SerializeField] protected List<DeliveryContainer<DeliverableColor>> _deliverableColors = new List<DeliveryContainer<DeliverableColor>>();
        [SerializeField] protected List<DeliveryContainer<DeliverableShape>> _deliverableShapes = new List<DeliveryContainer<DeliverableShape>>();
        [SerializeField] Delivery deliverAboveHead;
        
        [Header("ProjectilePrefab")]
        [SerializeField] ThrownDelivery _thrownDeliveryPrefab;
        [Header("ShootingPoint")]
        [SerializeField] Transform _shootingPointTransform;

        [SerializeField] float restoreChargeSpeed = 1.0f;

        //Temp message "no charge"
        [SerializeField] TextMeshProUGUI cantShootMessage;
        float messageDuration = 2.0f;
        Coroutine messageIsShowing;
        float currentNumberCharges;


        protected DeliveryContainer<DeliverableColor> _currentDeliveryColor;
        protected DeliveryContainer<DeliverableShape> _currentDeliveryShape;

        #region Properties

        
        public List<DeliveryContainer<DeliverableColor>> DeliverableColors => _deliverableColors;
        public List<DeliveryContainer<DeliverableShape>> DeliverableShapes => _deliverableShapes;
        
        public DeliveryContainer<DeliverableColor> CurrentDeliveryColor => _currentDeliveryColor;

        public DeliveryContainer<DeliverableShape> CurrentDeliveryShape => _currentDeliveryShape;

        public float CurrentNumberCharges => currentNumberCharges;

        public float MaxCharges => _maxCharges;

        #endregion"



        void Awake()
        {
            if (cantShootMessage == null)
            {
                print("Einar, don't forget to fix it");
            }
            
        }
        

        void Start()
        {
            NewColorSelected += (colorChange) =>
            {
                if (deliverAboveHead == null)
                {
                    Debug.LogError("you forgot to attach above head");
                }
                deliverAboveHead.DisplayDeliveryAboveHead(_currentDeliveryColor.GetContainerType(), _currentDeliveryShape.GetContainerType());
            };
            NewShapeSelected += (colorChange) =>
            {
                if (deliverAboveHead == null)
                {
                    Debug.LogError("you forgot to attach above head");
                }
                deliverAboveHead.DisplayDeliveryAboveHead(_currentDeliveryColor.GetContainerType(), _currentDeliveryShape.GetContainerType());
            };
            SetupDeliveries();
            ModifyCharge(_maxCharges);
            StartCoroutine(Charging());

            if (cantShootMessage!=null)
            {
                cantShootMessage.gameObject.SetActive(false);
                
            }
        }
        public void SetupDeliveries()
        {
            _currentDeliveryColor = _deliverableColors[0];
            _currentDeliveryShape = _deliverableShapes[0];
            NewColorSelected?.Invoke(_currentDeliveryColor);
            NewShapeSelected?.Invoke(_currentDeliveryShape);
            // foreach (var color in _deliverableColors)
            // {
            //     StartCoroutine(Charging(color));
            // }
            // foreach (var shape in _deliverableShapes)
            // {
            //     StartCoroutine(Charging(shape));
            // }
        }

        public void ResetSelection()
        {
            _currentDeliveryColor = _deliverableColors[0];
            _currentDeliveryShape = _deliverableShapes[0];
            NewColorSelected?.Invoke(_currentDeliveryColor);
            NewShapeSelected?.Invoke(_currentDeliveryShape);
        }
        //These 2 charges are added by particles
         // public void ModifyCharge(DeliverableColor deliverableColor, float amount, bool byParticle = false)
         // {
         //     foreach (var color in _deliverableColors)
         //     {
         //         if (color.GetContainerType() == deliverableColor)
         //         {
         //             color.ModifyCharge(amount,byParticle);
         //             break;
         //         }
         //     }
         // }
         // public void ModifyCharge(DeliverableShape deliverableShape, float amount, bool byParticle = false)
         // {
         //     foreach (var shape in _deliverableShapes)
         //     {
         //         if (shape.GetContainerType() == deliverableShape)
         //         {
         //             shape.ModifyCharge(amount, byParticle);
         //             break;
         //         }
         //     }
         // }
        IEnumerator Charging()
        {
            while (true)
            {
                ModifyCharge(Time.deltaTime *restoreChargeSpeed);
                // if (container.CurrentNumberCharges <container.MaxCharges)
                // {
                //     container.ModifyCharge(Time.deltaTime *restoreChargeSpeed);
                // }
                yield return new WaitForSeconds(0.1f);
            }
        }
        public void ModifyCharge(float amount, bool byParticle = false)
        {
            currentNumberCharges += amount;
            if (currentNumberCharges < 0)
            {
                currentNumberCharges = 0;
            }
            else if (currentNumberCharges > _maxCharges)
            {
                currentNumberCharges = _maxCharges;
            }
            ChargeModified?.Invoke(byParticle);
            
        }
        public virtual void SelectNextDelColor()
        {
            int nextIndex = _deliverableColors.IndexOf(_currentDeliveryColor);
            nextIndex++;
            if (nextIndex >= _deliverableColors.Count)
            {
                nextIndex = 0;
            }

            _currentDeliveryColor = _deliverableColors[nextIndex];
            NewColorSelected?.Invoke(_currentDeliveryColor);
            PlayColorSelectSound();
        }



        public virtual void SelectNextDelShape()
        {
            int nextIndex = _deliverableShapes.IndexOf(_currentDeliveryShape);
            nextIndex++;
            if (nextIndex >= _deliverableShapes.Count)
            {
                nextIndex = 0;
            }

            _currentDeliveryShape = _deliverableShapes[nextIndex];
            NewShapeSelected?.Invoke(_currentDeliveryShape);
            PlayShapeSelectSound();
        }

        bool CanShoot()
        {
            return currentNumberCharges >= 1.0f;
        }

        public virtual void ShootDelivery()
        {
            if (!CanShoot())
            {
                messageIsShowing = StartCoroutine(NoChargesMessage());
                return;
            }
            HasShot?.Invoke();
            
            ModifyCharge(_shootChargeSubtraction);

            ThrownDelivery projectile = Instantiate(_thrownDeliveryPrefab, _shootingPointTransform.position, Quaternion.identity);
            DeliveryInfo info = new DeliveryInfo(_currentDeliveryColor.GetContainerType(), _currentDeliveryShape.GetContainerType());
            projectile.Throw(info, _shootingPointTransform.forward);
            //projectile.GetComponent<Renderer>().material = _colorsMaterials[(int)CurrentDeliveryInfo.Color];
        }

        protected void InvokeNewColor()
        {
            NewColorSelected?.Invoke(_currentDeliveryColor);
        }

        protected void InvokeNewShape()
        {
            NewShapeSelected?.Invoke(_currentDeliveryShape);
        }



        IEnumerator NoChargesMessage()
        {
            PlayNotEnoughChargesSound();
            if (messageIsShowing !=null) yield break;
            if (cantShootMessage == null) yield break;

            cantShootMessage.gameObject.SetActive(true);
            yield return new WaitForSeconds(messageDuration);
            cantShootMessage.gameObject.SetActive(false);

            
            messageIsShowing = null;
        }

        void PlayNotEnoughChargesSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player Shooter/No Ammo");
        }

        protected void PlayShapeSelectSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player Shooter/Pick Shape");
        }
        protected void PlayColorSelectSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player Shooter/Pick Color");
        }
    }
}
