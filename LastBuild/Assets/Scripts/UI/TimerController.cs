using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace William
{
    public class TimerController : MonoBehaviour
    {
        public static event Action<float> OnTimerChange;
        public static event Action OnTimesUp;
        public float Timer;
        [SerializeField] GameObject _gameOverPanel;

        void FixedUpdate()
        {
            Timer -= Time.fixedDeltaTime;
            OnTimerChange?.Invoke(Timer);

            if (Timer < 0) TimesUp();
        }

        /// <summary>
        /// TODO
        /// </summary>
        void TimesUp()
        {
            Time.timeScale = 0;
            _gameOverPanel.SetActive(true);
            OnTimesUp?.Invoke();
        }
    }
}
