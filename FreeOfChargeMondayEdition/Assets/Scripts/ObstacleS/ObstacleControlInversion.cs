using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Driver;

namespace LevelDesign.Obstacles
{
    public class ObstacleControlInversion : Obstacle
    {
        [SerializeField] float _inversionTime;
        [SerializeField] bool _invertMovement, _invertColorShooting;

        protected override void ExecutePunishment()
        {
        }

        // IEnumerator InvertColorController()
        // {
        //     if (_invertColorShooting) yield break;
        //
        //     ColorController colorController = Player.GetComponent<ColorController>();
        //     if (colorController == null) yield break;
        //
        //
        //     colorController.DisableControls();
        //     colorController.SetupControls(true);
        //     
        //
        //     yield return new WaitForSeconds(_inversionTime);
        //     colorController.DisableControls();
        //     colorController.SetupControls(false);
        //     
        // }

        // IEnumerator InvertMovement()
        // {
        //     if (!_invertMovement) yield break;
        //
        //     //hello
        //     ShipMovement shipMovement = Player.GetComponent<ShipMovement>();
        //     if (shipMovement == null) yield break;
        //
        //     shipMovement.SetInversion(true);
        //     yield return new WaitForSeconds(_inversionTime);
        //     shipMovement.SetInversion(false);
        // }
    }
}