using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace William
{
    public class TimerUi : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _timerText;

        private void OnEnable()
        {
            TimerController.OnTimerChange += UpdateTimer;
        }

        /// <summary>
        /// Updates the timer.
        /// </summary>
        /// <param name="time">the time of the timer.</param>
        private void UpdateTimer(float time)
        {
            if(time % 60 < 10) _timerText.text = $"{Mathf.FloorToInt(time / 60)}:0{Mathf.FloorToInt(time % 60)}";
            else _timerText.text = $"{Mathf.FloorToInt(time / 60)}:{Mathf.FloorToInt(time % 60)}";
        }
    }
}
