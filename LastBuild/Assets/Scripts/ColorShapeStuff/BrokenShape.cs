using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeliveryZoneInfo
{
    public class BrokenShape : MonoBehaviour
    {
        [SerializeField] float _lifeTime = 10.0f;
        [SerializeField] Rigidbody[] _rigidbodies;
        [SerializeField] float _downForce = 10;

        void Start()
        {
            foreach (var rb in _rigidbodies)
            {
                rb.AddForce(Vector3.down * _downForce);
            }
            Collider parent = GetComponentInParent<Collider>();
            parent.enabled = false;
            Destroy(parent.gameObject,_lifeTime);
        }
    }
    
}
