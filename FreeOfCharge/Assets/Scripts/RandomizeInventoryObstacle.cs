using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Einar.Core;

namespace William
{
    public class RandomizeInventoryObstacle : Obstacle
    {
        protected override void ExecutePunishment()
        {
            var player = Player.GetComponent<ColorPicker>();
            for (int i = 0; i < 3; i++)
            {
                player.DiscardDelivery();
            }
        }
    }
}
