using System;
using UnityEngine;

namespace DeliveryZoneInfo
{


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
}