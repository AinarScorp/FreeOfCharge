using DeliveryZoneInfo;
using System;

namespace Tutorial
{
    public class DeliveryTutorial : Delivery
    {
        public static event Action OnCompletedDelivery;
        public static DeliverableColor[] AllowedColors;
        public static DeliverableShape[] AllowedShapes;

        void Start()
        {
            InitializeDelivery(AllowedColors[UnityEngine.Random.Range(0, AllowedColors.Length)], AllowedShapes[UnityEngine.Random.Range(0, AllowedShapes.Length)]);
        }

        void OnEnable()
        {
            TutorialManager.OnNextSection += DisableThis;
        }

        void OnDisable()
        {
            TutorialManager.OnNextSection -= DisableThis;
        }

        /// <summary>
        /// Disables this gameObject.
        /// </summary>
        void DisableThis()
        { 
            gameObject.SetActive(false);
        }

        public override void CompleteDelivery(DeliveryInfo deliveredInfo)
        {
            base.CompleteDelivery(deliveredInfo);
            if(thisDeliveryInfo.Color == deliveredInfo.Color && thisDeliveryInfo.Shape == deliveredInfo.Shape)
            {
                OnCompletedDelivery?.Invoke();
            }
        }
    }
}