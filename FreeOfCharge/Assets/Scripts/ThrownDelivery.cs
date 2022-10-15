using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using William;

namespace William
{
    public class ThrownDelivery : MonoBehaviour
    {
        bool _thrown = false;
        Vector3 _startPoint;
        Vector3 _endPoint => _delivery.transform.position;

        [SerializeField] float _animationSpeed;

        Delivery _delivery;
        DeliveryInfo _deliveryInfo;

        float _lerp = 0f;

        /// <summary>
        /// Throws a projectile towards a delivery point.
        /// </summary>
        /// <param name="delivery">the delivery point.</param>
        /// <param name="color">the info of the thrown delivery.</param>
        public void Throw(Delivery delivery, DeliveryInfo info)
        {
            _startPoint = transform.position;
            _delivery = delivery;
            _deliveryInfo = info;
            _thrown = true;
        }

        void FixedUpdate()
        {
            if (!_thrown) return;

            _lerp += Time.fixedDeltaTime * _animationSpeed;
            transform.position = Vector3.Lerp(_startPoint, _endPoint, _lerp);

            if (_lerp >= 1)
            {
                _delivery.CompleteDelivery(_deliveryInfo.Color == _delivery.DeliveryInfo.Color, _deliveryInfo.Shape == _delivery.DeliveryInfo.Shape);
                gameObject.SetActive(false);
            }
        }
    }
}