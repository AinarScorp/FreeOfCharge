using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Driver;
using Player.Shooter;

namespace LevelDesign.Obstacles
{
    public class ObstacleReduceCharges : Obstacle
    {
        [SerializeField] float _chargeReduction = 0.5f;
        protected override void ExecutePunishment()
        {
            if (Player.TryGetComponent(out ColorPicker colorPicker))
            {
                foreach (var color in colorPicker.DeliverableColors)
                {
                    colorPicker.ModifyCharge(color.GetContainerType(), -_chargeReduction);
                }

                foreach (var shape in colorPicker.DeliverableShapes)
                {
                    colorPicker.ModifyCharge(shape.GetContainerType(), -_chargeReduction);
                }
            }
            else
                Debug.LogError("Something is wrong with obstacle");
        }


    }
}