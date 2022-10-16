using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using William;

namespace William
{
    public class ThrownDelivery : MonoBehaviour
    {

        [SerializeField] float _shotingSpeed;
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] Delivery _delivery;
        DeliveryInfo _deliveryInfo;
        [SerializeField]LayerMask _layerMask;


        public void Throw(DeliveryInfo info, Vector3 direction)
        {
            _deliveryInfo = info;
            _rigidbody.velocity = direction.normalized * _shotingSpeed;
            MeshRenderer meshRenderer = Instantiate(_delivery.Shapes[(int)_deliveryInfo.Shape], transform);
            meshRenderer.material = _delivery.ColorMaterials[(int)_deliveryInfo.Color];
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
                    this.gameObject.SetActive(false);
                }
                
            }
        }

        void FixedUpdate()
        {
            
        }
    }
}