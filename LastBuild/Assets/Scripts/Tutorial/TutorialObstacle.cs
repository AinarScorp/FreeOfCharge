using LevelDesign.Obstacles;
using UnityEngine;

namespace Tutorial
{
    public class TutorialObstacle : Obstacle
    {
        [SerializeField] GameObject _clearTrigger;

        protected override void ExecutePunishment()
        {
            _clearTrigger.SetActive(false);
        }

        private void OnEnable()
        {
            TutorialManager.OnNextSection += Clear;
        }

        private void OnDisable()
        {
            TutorialManager.OnNextSection -= Clear;
        }

        /// <summary>
        /// Disables clear trigger and this gameObject.
        /// </summary>
        void Clear()
        {
            _clearTrigger.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}