using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeliveryZoneInfo;
using UnityEngine.UI;

namespace Ui.DeliverInfo
{
    
    public class CountDownImage : MonoBehaviour
    {

        [SerializeField] GameObject _flashImage;
        [SerializeField] Image _image;
        DeliveryContainer<DeliverableColor> deliveryContainerColor;
        DeliveryContainer<DeliverableShape> deliveryContainerShape;

        public DeliveryContainer<DeliverableColor> DeliveryContainerColor => deliveryContainerColor;

        public DeliveryContainer<DeliverableShape> DeliveryContainerShape => deliveryContainerShape;

        public RectTransform ImageTransform => _image.rectTransform;



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

        public void UpdateCountDown(bool updatedByParticles = false)
        {
            if (updatedByParticles)
            {
                SpawnFlashVFX();
            }
            if (deliveryContainerColor !=null)
            {
                _image.fillAmount = (deliveryContainerColor.CurrentNumberCharges / deliveryContainerColor.MaxCharges);
            }
            else if (deliveryContainerShape !=null)
            {
                _image.fillAmount = (deliveryContainerShape.CurrentNumberCharges / deliveryContainerShape.MaxCharges);
            }
        }

        void SpawnFlashVFX()
        {
            if (!_flashImage) return;
            var vfx = Instantiate(_flashImage, transform);
            ParticleSystem particle = vfx.GetComponentInChildren<ParticleSystem>();
            Destroy(vfx.gameObject,particle.main.duration + particle.main.startLifetime.constantMax);

        }
    }
}
