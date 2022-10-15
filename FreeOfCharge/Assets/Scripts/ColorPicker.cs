using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Einar.Inputs;
using UnityEngine.Events;
using Einar.UI;

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

    public class ColorPicker : MonoBehaviour
    {
        public static UnityEvent<List<DeliveryInfo>> OnInventoryChange = new UnityEvent<List<DeliveryInfo>>();
        public event Action<DeliverableColor> NewColorSelected;
        public event Action<DeliverableShape> NewShapeSelected;

        [SerializeField] List<DeliverableColor> _deliverableColors = new List<DeliverableColor>();

        [SerializeField] List<DeliverableShape> _deliverableShapes = new List<DeliverableShape>();

        List<DeliveryInfo> _deliveryInfoList = new List<DeliveryInfo>();
        DeliveryInfo CurrentDeliveryInfo => _deliveryInfoList[0];

        [SerializeField] ThrownDelivery _thrownDeliveryPrefab;


        [Tooltip("Red, Blue, Yellow")] [SerializeField]
        Material[] _colorsMaterials;

        List<Delivery> _deliveriesInRange = new List<Delivery>();
        public Delivery DeliveryInRange => GetBestDeliveryOption();

        public List<DeliverableColor> DeliverableColors => _deliverableColors;

        public List<DeliverableShape> DeliverableShapes => _deliverableShapes;


        [SerializeField] CrosshairFollowTarget _crosshairFollowTarget;
        [SerializeField] Transform _shootingPointTransform;

        DeliverableColor _currentDeliveryColor;
        DeliverableShape _currentDeliveryShape;
        public DeliverableColor CurrentDeliveryColor => _currentDeliveryColor;

        public DeliverableShape CurrentDeliveryShape => _currentDeliveryShape;

        void Start()
        {
            SetupDeliveries();
        }

        /// <summary>
        /// Removes the current delivery.
        /// </summary>
        /// <returns>the discarded delivery.</returns>
        public DeliveryInfo RemoveDelivery()
        {
            var delivery = _deliveryInfoList[0];
            _deliveryInfoList.Remove(delivery);
            OnInventoryChange?.Invoke(_deliveryInfoList);
            return delivery;
        }

        /// <summary>
        /// Removes the current delivery and adds a random delivery to the end of the inventory.
        /// </summary>
        public void DiscardDelivery()
        {
            RemoveDelivery();
            SetupDeliveries();
        }

        /// <summary>
        /// Adds a random delivery to the inventory.
        /// </summary>
        public void SetupDeliveries()
        {
            _currentDeliveryColor = _deliverableColors[0];
            _currentDeliveryShape = _deliverableShapes[0];
            NewColorSelected?.Invoke(_currentDeliveryColor);
            NewShapeSelected?.Invoke(_currentDeliveryShape);
            OnInventoryChange?.Invoke(_deliveryInfoList);
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

        /// <summary>
        /// Gets the best delivery point in range of the player.
        /// </summary>
        /// <returns>the best delivery point.</returns>
        Delivery GetBestDeliveryOption()
        {
            return null;
            // if (_deliveriesInRange.Count <= 0) return null;
            //
            // var correctColors = new List<Delivery>();
            //
            // foreach (var delivery in _deliveriesInRange)
            // {
            //     if (delivery.DeliveryInfo.Color == CurrentDeliveryInfo.Color) correctColors.Add(delivery);
            // }
            //
            // if (correctColors.Count == 0) return GetClosestDeliveryPoint(_deliveriesInRange);
            // else return GetClosestDeliveryPoint(correctColors);
        }

        /// <summary>
        /// Gets the closest delivery point from a list.
        /// </summary>
        /// <param name="points">the list of delivery points.</param>
        /// <returns>the closest delivery point.</returns>
        Delivery GetClosestDeliveryPoint(List<Delivery> points)
        {
            var closest = points[0];
            var closestRange = Vector3.Distance(transform.position, points[0].transform.position);
            for (int i = 0; i < points.Count; i++)
            {
                var distance = Vector3.Distance(transform.position, points[i].transform.position);
                if (distance < closestRange)
                {
                    closest = points[i];
                    closestRange = distance;
                }
            }

            return closest;
        }

        /// <summary>
        /// Delivers the current delivery to a nearby delivery point.
        /// </summary>
        public void ShootDelivery()
        {
            //make it just shoot forward

            ThrownDelivery projectile = Instantiate(_thrownDeliveryPrefab, transform.position, Quaternion.identity);
            DeliveryInfo info = new DeliveryInfo(_currentDeliveryColor, _currentDeliveryShape);
            projectile.Throw(info, _shootingPointTransform.forward);
            //projectile.GetComponent<Renderer>().material = _colorsMaterials[(int)CurrentDeliveryInfo.Color];

        }

        //these two you should remove




        /// <summary>
        /// Updates the crosshair.
        /// </summary>
        void UpdateCrosshair()
        {
            foreach (var delivery in _deliveriesInRange)
            {
                delivery.ToggleParticle(false);
            }

            if (DeliveryInRange != null)
            {
                DeliveryInRange.ToggleParticle(true);
            }

            if (!_crosshairFollowTarget) return;

            _crosshairFollowTarget.gameObject.SetActive(DeliveryInRange);
        }
    }
}