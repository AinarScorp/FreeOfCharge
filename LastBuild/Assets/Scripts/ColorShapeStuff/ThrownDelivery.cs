using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using William;

namespace DeliveryZoneInfo
{
    public class ThrownDelivery : MonoBehaviour
    {
        [SerializeField] float _shotingSpeed;
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] Delivery _delivery;
        DeliveryInfo _deliveryInfo;
        [SerializeField] LayerMask _layerMask;
        [SerializeField] [Range(0.0f, 1.0f)] float _shringScale;
        [SerializeField] float _shrinkingSpeed;


        public void Throw(DeliveryInfo info, Vector3 direction)
        {
            _deliveryInfo = info;
            _rigidbody.velocity = direction.normalized * _shotingSpeed;
            MeshRenderer meshRenderer = Instantiate(_delivery.Shapes[(int)_deliveryInfo.Shape], transform);
            meshRenderer.material = _delivery.ColorMaterials[(int)_deliveryInfo.Color];
            StartCoroutine(Shrink());
        }

        void OnTriggerEnter(Collider other)
        {
            if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                other.TryGetComponent(out Delivery delivery);
                if (delivery != null)
                {
                    delivery.CompleteDelivery(_deliveryInfo);
                    StopAllCoroutines();
                    Vanish();
                }
            }
        }

        IEnumerator Shrink()
        {
            //I will do bad thing - modify scale
            Vector3 startScale = transform.localScale;
            Vector3 endScale = startScale * _shringScale;
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * _shrinkingSpeed;
                transform.localScale = Vector3.Lerp(startScale, endScale, percent);
                yield return null;
            }

            transform.localScale = endScale;
            Vanish();
        }

        void Vanish()
        {
            Destroy(this.gameObject);
        }
    }
}