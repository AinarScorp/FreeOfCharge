using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{


    public enum TutorialColliderPoint
    {
        MoveLeft,
        MoveRight,
        Jump
    }

    public class TutorialCollider : MonoBehaviour
    {
        [SerializeField] TutorialColliderPoint tutorialPointType;
        TutorialManager _tutorialManager;
        [SerializeField] LayerMask _layerMask;

        void Awake()
        {
            _tutorialManager = FindObjectOfType<TutorialManager>();
        }

        void OnTriggerEnter(Collider other)
        {
            if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                return;
                _tutorialManager.TouchedTutorialCollider(tutorialPointType);
                this.gameObject.SetActive(false);
            }
        }
    }
}