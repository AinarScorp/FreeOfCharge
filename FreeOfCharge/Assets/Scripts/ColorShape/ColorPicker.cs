using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Einar.Inputs;
using TMPro;
using UnityEngine.Events;

namespace William
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

    [Serializable]
    public class DeliveryContainer<T>
    {
        [SerializeField] T deliveryType;
        [SerializeField] float maxCharges;
        [SerializeField] float currentNumberCharges;
        public event Action ChargeSubtracted;
        public event Action ChargeAdded;

        public DeliveryContainer(T deliveryType, int maxCharges)
        {
            this.deliveryType = deliveryType;
            this.maxCharges = maxCharges;
            currentNumberCharges = maxCharges;
        }

        public float MaxCharges => maxCharges;

        public float CurrentNumberCharges => currentNumberCharges;



        public T GetType() => deliveryType;

        public bool CanShoot()
        {
            return currentNumberCharges >= 1.0f;
        }

        public void SubtractCharge()
        {
            currentNumberCharges--;
            if (currentNumberCharges < 0)
            {
                currentNumberCharges = 0;
            }
            ChargeSubtracted?.Invoke();
        }

        public void AddCharge(float amount)
        {
            currentNumberCharges += amount;
            if (currentNumberCharges > maxCharges)
            {
                currentNumberCharges = maxCharges;
            }
            ChargeAdded?.Invoke();
        }
    }

    public class ColorPicker : MonoBehaviour
    {
        public event Action<DeliveryContainer<DeliverableColor>> NewColorSelected;
        public event Action<DeliveryContainer<DeliverableShape>> NewShapeSelected;


        [SerializeField] List<DeliveryContainer<DeliverableColor>> _deliverableColors = new List<DeliveryContainer<DeliverableColor>>();

        [SerializeField] List<DeliveryContainer<DeliverableShape>> _deliverableShapes = new List<DeliveryContainer<DeliverableShape>>();


        [SerializeField] ThrownDelivery _thrownDeliveryPrefab;



        public List<DeliveryContainer<DeliverableColor>> DeliverableColors => _deliverableColors;

        public List<DeliveryContainer<DeliverableShape>> DeliverableShapes => _deliverableShapes;


        [SerializeField] Transform _shootingPointTransform;

        DeliveryContainer<DeliverableColor> _currentDeliveryColor;
        DeliveryContainer<DeliverableShape> _currentDeliveryShape;
        public DeliveryContainer<DeliverableColor> CurrentDeliveryColor => _currentDeliveryColor;

        public DeliveryContainer<DeliverableShape> CurrentDeliveryShape => _currentDeliveryShape;

        [SerializeField] float restoreChargeSpeed = 1.0f;
        [SerializeField] TextMeshProUGUI cantShootMessage;
        Coroutine messageIsShowing;
        float messageDuration = 2.0f;
        [SerializeField] Delivery deliverAboveHead;

        void Start()
        {
            NewColorSelected += (colorChange) =>
            {
                if (deliverAboveHead == null)
                {
                    Debug.LogError("you forgot to attach above head");
                }
                deliverAboveHead.DisplayDeliveryAboveHead(_currentDeliveryColor.GetType(), _currentDeliveryShape.GetType());
            };
            NewShapeSelected += (colorChange) =>
            {
                if (deliverAboveHead == null)
                {
                    Debug.LogError("you forgot to attach above head");
                }
                deliverAboveHead.DisplayDeliveryAboveHead(_currentDeliveryColor.GetType(), _currentDeliveryShape.GetType());
            };
            SetupDeliveries();
            cantShootMessage.gameObject.SetActive(false);
        }

        public void SetupDeliveries()
        {
            _currentDeliveryColor = _deliverableColors[0];
            _currentDeliveryShape = _deliverableShapes[0];
            NewColorSelected?.Invoke(_currentDeliveryColor);
            NewShapeSelected?.Invoke(_currentDeliveryShape);
            foreach (var color in _deliverableColors)
            {
                StartCoroutine(Charging(color));
            }
            foreach (var shape in _deliverableShapes)
            {
                StartCoroutine(Charging(shape));
            }
        }

        public void AddCharge(DeliverableColor deliverableColor)
        {
            foreach (var color in _deliverableColors)
            {
                if (color.GetType() == deliverableColor)
                {
                    color.AddCharge(0.1f);
                    print("yey");
                }
            }
        }
        public void AddCharge(DeliverableShape deliverableShape)
        {
            foreach (var shape in _deliverableShapes)
            {
                if (shape.GetType() == deliverableShape)
                {
                    shape.AddCharge(0.1f);
                    print("yey");

                }
            }
        }
        IEnumerator Charging<T>(DeliveryContainer<T> container)
        {
            while (true)
            {
                if (container.CurrentNumberCharges <container.MaxCharges)
                {
                    container.AddCharge(Time.deltaTime *restoreChargeSpeed);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        public void SelectNextDelColor()
        {
            int nextIndex = _deliverableColors.IndexOf(_currentDeliveryColor);
            nextIndex++;
            if (nextIndex >= _deliverableColors.Count)
            {
                nextIndex = 0;
            }

            _currentDeliveryColor = _deliverableColors[nextIndex];
            NewColorSelected?.Invoke(_currentDeliveryColor);
        }

        public void SelectNextDelShape()
        {
            int nextIndex = _deliverableShapes.IndexOf(_currentDeliveryShape);
            nextIndex++;
            if (nextIndex >= _deliverableShapes.Count)
            {
                nextIndex = 0;
            }

            _currentDeliveryShape = _deliverableShapes[nextIndex];
            NewShapeSelected?.Invoke(_currentDeliveryShape);
        }


        public void ShootDelivery()
        {
            //make it just shoot forward
            if (!_currentDeliveryColor.CanShoot() || !_currentDeliveryShape.CanShoot())
            {
                
                messageIsShowing = StartCoroutine(NoChargesMessage());
                
                return;
            }
            _currentDeliveryColor.SubtractCharge();
            _currentDeliveryShape.SubtractCharge();

            ThrownDelivery projectile = Instantiate(_thrownDeliveryPrefab, transform.position, Quaternion.identity);
            DeliveryInfo info = new DeliveryInfo(_currentDeliveryColor.GetType(), _currentDeliveryShape.GetType());
            projectile.Throw(info, _shootingPointTransform.forward);
            //projectile.GetComponent<Renderer>().material = _colorsMaterials[(int)CurrentDeliveryInfo.Color];
        }

        IEnumerator NoChargesMessage()
        {
            if (messageIsShowing !=null) yield break;

            cantShootMessage.gameObject.SetActive(true);
            yield return new WaitForSeconds(messageDuration);
            cantShootMessage.gameObject.SetActive(false);

            
            messageIsShowing = null;
        }
    }
}