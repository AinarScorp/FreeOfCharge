using System;
using UnityEngine;

namespace DeliveryZoneInfo
{


    [Serializable]
    public class DeliveryContainer<T>
    {
        [SerializeField] T deliveryType;
        // [SerializeField] float maxCharges;
        // [SerializeField] float currentNumberCharges;
        //public event Action<bool> ChargeModified;
        

        public DeliveryContainer(T deliveryType)
        {
            this.deliveryType = deliveryType;
            // this.maxCharges = maxCharges;
            // currentNumberCharges = maxCharges;
        }


        // public float MaxCharges => maxCharges;
        //
        // public float CurrentNumberCharges => currentNumberCharges;
        //


        public T GetContainerType() => deliveryType;

        // public bool CanShoot()
        // {
        //     return currentNumberCharges >= 1.0f;
        // }

        // public void ModifyCharge(float amount, bool byParticle = false)
        // {
        //     currentNumberCharges += amount;
        //     if (currentNumberCharges < 0)
        //     {
        //         currentNumberCharges = 0;
        //     }
        //     else if (currentNumberCharges > maxCharges)
        //     {
        //         currentNumberCharges = maxCharges;
        //     }
        //     ChargeModified?.Invoke(byParticle);
        //     
        // }


    }
}