using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace William
{
    public class ColorInventoryUi : MonoBehaviour
    {
        [SerializeField] Image[] _inventorySlotImages;

        void OnEnable()
        {
            ColorPicker.OnInventoryChange.AddListener(UpdateInventory);
        }

        void OnDisable()
        {
            ColorPicker.OnInventoryChange.RemoveListener(UpdateInventory);
        }

        /// <summary>
        /// Updates the inventory ui.
        /// </summary>
        /// <param name="inventory">the inventory.</param>
        void UpdateInventory(List<DeliveryInfo> inventory)
        {
            //TODO Add shapes to inventory
            for (int i = 0; i < inventory.Count; i++)
            {
                switch(inventory[i].Color)
                {
                    case DeliverableColor.Red:
                    _inventorySlotImages[i].color = Color.red;
                        break;

                    case DeliverableColor.Blue:
                    _inventorySlotImages[i].color = Color.blue;
                        break;

                    case DeliverableColor.Yellow:
                    _inventorySlotImages[i].color = Color.yellow;
                        break;
                }
            }
        }
    }
}
