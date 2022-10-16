using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using William;

public class DeliveryDestroyer : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;
    [SerializeField] ParticleSystem hitParticle;
    void OnTriggerEnter(Collider other)
    {
        if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if (other.TryGetComponent(out Delivery delivery))
            {
                if (hitParticle !=null)
                {
                    Instantiate(hitParticle, delivery.transform.position, hitParticle.transform.rotation, transform);
                }
                delivery.gameObject.SetActive(false);
            }


            
        }

    }
}
