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
        public static ColorPicker Instance;

        public static UnityEvent<List<DeliveryInfo>> OnInventoryChange = new UnityEvent<List<DeliveryInfo>>();

        //List<DeliverableColor> _colorList = new List<DeliverableColor>();
        List<DeliveryInfo> _deliveryInfoList = new List<DeliveryInfo>();
        DeliveryInfo CurrentDeliveryInfo => _deliveryInfoList[0];

        [SerializeField] ThrownDelivery _thrownDeliveryPrefab;


        [Tooltip("Red, Blue, Yellow")]
        [SerializeField] Material[] _colorsMaterials;

        List<Delivery> _deliveriesInRange = new List<Delivery>();
        public Delivery DeliveryInRange => GetBestDeliveryOption();

        [SerializeField] CrosshairFollowTarget _crosshairFollowTarget;


        void Awake()
        {
            if(Instance == null) Instance = this;    
            else Destroy(gameObject);
        }

        void Start()
        {
            AddRandomDelivery();
            AddRandomDelivery();
            AddRandomDelivery();
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
            AddRandomDelivery();
        }

        /// <summary>
        /// Adds a random delivery to the inventory.
        /// </summary>
        public void AddRandomDelivery()
        {
            var delivery = new DeliveryInfo((DeliverableColor)Random.Range(0, 3), (DeliverableShape)Random.Range(0, 3));
            _deliveryInfoList.Add(delivery);
            OnInventoryChange?.Invoke(_deliveryInfoList);
        }

        /// <summary>
        /// Gets the best delivery point in range of the player.
        /// </summary>
        /// <returns>the best delivery point.</returns>
        Delivery GetBestDeliveryOption()
        {
            if (_deliveriesInRange.Count <= 0) return null;

            var correctColors = new List<Delivery>();

            foreach (var delivery in _deliveriesInRange)
            {
                if (delivery.DeliveryInfo.Color == CurrentDeliveryInfo.Color) correctColors.Add(delivery);
            }

            if(correctColors.Count == 0) return GetClosestDeliveryPoint(_deliveriesInRange);
            else return GetClosestDeliveryPoint(correctColors);
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
        public void Deliver()
        {
            if (!DeliveryInRange) return;

            ThrownDelivery thrown = Instantiate(_thrownDeliveryPrefab, transform.position, Quaternion.identity);
            thrown.Throw(DeliveryInRange, CurrentDeliveryInfo);
            thrown.GetComponent<Renderer>().material = _colorsMaterials[(int)CurrentDeliveryInfo.Color];

            DiscardDelivery();
            _deliveriesInRange.Remove(DeliveryInRange);
            UpdateCrosshair();
        }

        public IEnumerator DisableFor(float duration)
        {
            this.enabled = false;
            yield return new WaitForSeconds(duration);
            this.enabled = true;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.tag != "DeliveryPoint") return;

            var deliveryPoint = col.GetComponent<Delivery>();

            if (deliveryPoint.DeliveryStarted) return;

            _deliveriesInRange.Add(deliveryPoint);
            UpdateCrosshair();

        }

        void OnTriggerExit(Collider col)
        {
            if (col.tag != "DeliveryPoint") return;
            Delivery delivery = col.GetComponent<Delivery>();
            delivery.ToggleParticle(false);
            _deliveriesInRange.Remove(delivery);
            
            UpdateCrosshair();
        }

        /// <summary>
        /// Updates the crosshair.
        /// </summary>
        void UpdateCrosshair()
        {
            foreach (var delivery in _deliveriesInRange)
            {
                delivery.ToggleParticle(false);
            }

            if (DeliveryInRange !=null)
            {
                DeliveryInRange.ToggleParticle(true);
            }
            if (!_crosshairFollowTarget) return;
            
            _crosshairFollowTarget.gameObject.SetActive(DeliveryInRange);
            
        }
    }
}
