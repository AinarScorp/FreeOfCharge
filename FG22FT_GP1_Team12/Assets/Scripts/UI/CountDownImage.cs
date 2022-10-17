using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeliveryZoneInfo;
using UnityEngine.UI;

namespace Ui.DeliverInfo
{
    
    public class CountDownImage : MonoBehaviour
    {
        [SerializeField] Image _image;
        DeliveryContainer<DeliverableColor> deliveryContainerColor;
        DeliveryContainer<DeliverableShape> deliveryContainerShape;

        public void SetupCountDown(DeliveryContainer<DeliverableColor> deliveryContainer) //THIS IS WHERE I WANT TO ASSIGN IT
        {
            deliveryContainerColor = deliveryContainer;
            UpdateCountDown();
        }
        public void SetupCountDown(DeliveryContainer<DeliverableShape> deliveryContainer) //THIS IS WHERE I WANT TO ASSIGN IT
        {
            deliveryContainerShape = deliveryContainer;
            UpdateCountDown();
        }

        public void UpdateCountDown()
        {
            if (deliveryContainerColor !=null)
            {
                _image.fillAmount = (deliveryContainerColor.CurrentNumberCharges / deliveryContainerColor.MaxCharges);
            }
            else if (deliveryContainerShape !=null)
            {
                _image.fillAmount = (deliveryContainerShape.CurrentNumberCharges / deliveryContainerShape.MaxCharges);

            }
        }
    }
}
