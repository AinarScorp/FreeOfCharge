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
                colorPicker.ModifyCharge(-_chargeReduction);
            }
        }


    }
}