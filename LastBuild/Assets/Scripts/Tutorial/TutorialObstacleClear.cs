using System;
using UnityEngine;

namespace Tutorial
{
    public class TutorialObstacleClear : MonoBehaviour
    {
        [SerializeField] LayerMask _layerMask;

        public static event Action OnObstacleClear;

        private void OnTriggerEnter(Collider other)
        {
            if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                OnObstacleClear?.Invoke();
            }
        }
    }
}