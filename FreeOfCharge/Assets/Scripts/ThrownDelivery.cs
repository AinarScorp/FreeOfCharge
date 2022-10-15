using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using William;

namespace William
{
    public class ThrownDelivery : MonoBehaviour
    {

        [SerializeField] float _animationSpeed;
        [SerializeField] float _shotingSpeed;
        [SerializeField] Rigidbody _rigidbody;
        Delivery _delivery;
        DeliveryInfo _deliveryInfo;
        [SerializeField]LayerMask _layerMask;


        /// <summary>
        /// Throws a projectile towards a delivery point.
        /// </summary>
        /// <param name="delivery">the delivery point.</param>
        /// <param name="color">the info of the thrown delivery.</param>
        
        public void Throw(DeliveryInfo info, Vector3 direction)
        {
            _deliveryInfo = info;
            _rigidbody.velocity = direction.normalized * _shotingSpeed;
        }
        
        void OnTriggerEnter(Collider other)
        {
            if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                
                print("I hit " + other.name);
                other.TryGetComponent(out Delivery delivery);
                if (delivery!=null)
                {
                    delivery.CompleteDelivery(_deliveryInfo);
                }
            }
        }

        void FixedUpdate()
        {
            
        }
    }
}