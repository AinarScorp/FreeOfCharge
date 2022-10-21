using UnityEngine;

namespace Tutorial
{
    public class TutorialCollider : MonoBehaviour
    {
        [SerializeField] TutorialSection _tutorialSection;
        TutorialManager _tutorialManager;
        [SerializeField] LayerMask _layerMask;

        void Awake()
        {
            _tutorialManager = FindObjectOfType<TutorialManager>();
            TutorialManager.OnNextSection += DisableThis;
        }

        void OnTriggerEnter(Collider other)
        {
            if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                _tutorialManager.SectionDone(_tutorialSection);
            }
        }

        /// <summary>
        /// Disables this gameObject.
        /// </summary>
        void DisableThis()
        {
            gameObject.SetActive(false);
            TutorialManager.OnNextSection -= DisableThis;
        }
    }
}