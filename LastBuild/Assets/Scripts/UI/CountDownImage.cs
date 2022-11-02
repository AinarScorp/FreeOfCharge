using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeliveryZoneInfo;
using Player.Shooter;
using UnityEngine.UI;

namespace Ui.DeliverInfo
{
    public class CountDownImage : MonoBehaviour
    {
        [SerializeField] GameObject _flashImage;

        [SerializeField] Image _image;
        // DeliveryContainer<DeliverableColor> deliveryContainerColor;
        // DeliveryContainer<DeliverableShape> deliveryContainerShape;
        //
        // public DeliveryContainer<DeliverableColor> DeliveryContainerColor => deliveryContainerColor;
        //
        // public DeliveryContainer<DeliverableShape> DeliveryContainerShape => deliveryContainerShape;

        public RectTransform ImageTransform => _image.rectTransform;

        ColorPicker _colorPicker;

        public void SetupCountDown(ColorPicker colorPicker)
        {
            _colorPicker = colorPicker;
            _colorPicker.ChargeModified += UpdateCountDown;
            UpdateCountDown();
        }

        // public void SetupCountDown(DeliveryContainer<DeliverableColor> deliveryContainer) //THIS IS WHERE I WANT TO ASSIGN IT
        // {
        //     deliveryContainerColor = deliveryContainer;
        //     UpdateCountDown();
        // }
        // public void SetupCountDown(DeliveryContainer<DeliverableShape> deliveryContainer) //THIS IS WHERE I WANT TO ASSIGN IT
        // {
        //     deliveryContainerShape = deliveryContainer;
        //     UpdateCountDown();
        // }
        //

        public void UpdateCountDown(bool updatedByParticles = false)
        {
            if (updatedByParticles)
            {
                SpawnFlashVFX();
            }

            _image.fillAmount = (_colorPicker.CurrentNumberCharges / _colorPicker.MaxCharges);
            // if (deliveryContainerColor !=null)
            // {
            // }
            // else if (deliveryContainerShape !=null)
            // {
            //     _image.fillAmount = (colorPicker.CurrentNumberCharges / colorPicker.MaxCharges);
            // }
        }

        void SpawnFlashVFX()
        {
            if (!_flashImage) return;
            var vfx = Instantiate(_flashImage, transform);
            vfx.transform.localScale = this.transform.localScale;
            ParticleSystem particle = vfx.GetComponentInChildren<ParticleSystem>();
            Destroy(vfx.gameObject, particle.main.duration + particle.main.startLifetime.constantMax);
        }
    }
}