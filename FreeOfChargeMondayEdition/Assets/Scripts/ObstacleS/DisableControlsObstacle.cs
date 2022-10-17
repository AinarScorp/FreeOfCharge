using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Driver;
using Player.Shooter;

namespace LevelDesign.Obstacles
{
    public class DisableControlsObstacle : Obstacle
    {
        [SerializeField] float _disableDuration;

        enum ControlsDisabled
        {
            Movement,
            Delivery
        }

        [SerializeField] ControlsDisabled _controlsDisabled;

        protected override void ExecutePunishment()
        {
            switch (_controlsDisabled)
            {
                case ControlsDisabled.Movement:
                    Player.GetComponent<ShipMovement>().enabled = false;
                    break;

                case ControlsDisabled.Delivery:
                    Player.GetComponent<ColorPicker>().enabled = false;
                    //Player.GetComponent<ColorController>().enabled = false;
                    break;
            }

            StartCoroutine(EnableAfter(_disableDuration));
        }

        /// <summary>
        /// Enables the component after a duration.
        /// </summary>
        /// <param name="duration">ther duration.</param>
        IEnumerator EnableAfter(float duration)
        {
            yield return new WaitForSeconds(duration);

            switch(_controlsDisabled)
            {
                case ControlsDisabled.Movement:
                    Player.GetComponent<ShipMovement>().enabled = true;
                    break;

                case ControlsDisabled.Delivery:
                    Player.GetComponent<ColorPicker>().enabled = true;
                    //Player.GetComponent<ColorController>().enabled = true;
                    break;
            }
        }
    }
}
