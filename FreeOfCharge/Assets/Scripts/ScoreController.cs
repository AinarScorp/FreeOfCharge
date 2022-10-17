using Einar.Core;
using System;
using UnityEngine;

namespace William
{
    public class ScoreController : MonoBehaviour
    {
        public static ScoreController Instance;
        public static event Action<int> OnScoreAdded;
        public static event Action<int> OnScoreRemoved;
        public int Score;
        [SerializeField] int _scorePerDelivery;
        float _scoreMultiplier;

        void Awake()
        {
            Instance = this;
        }



        /// <summary>
        /// Change the score.
        /// </summary>
        /// <param name="sucessfulColor">true if the delivery was sucessful.</param>
        public void ChangeScore(bool sucessfulColor, bool sucessfulShape)
        {
            //TODO add sucessful shape
            if (sucessfulColor) AddScore((int)(_scorePerDelivery * _scoreMultiplier));
            else RemoveScore(_scorePerDelivery);
        }

        /// <summary>
        /// Adds an amount to the score.
        /// </summary>
        /// <param name="amount">the amount to add.</param>
        void AddScore(int amount)
        {
            Score += amount;
            OnScoreAdded?.Invoke(Score);
        }

        /// <summary>
        /// Removes an amount from the score.
        /// </summary>
        /// <param name="amount">the amount to remove.</param>
        public void RemoveScore(int amount)
        {
            Score -= amount;
            OnScoreRemoved?.Invoke(Score);
        }

        /// <summary>
        /// Sets the score multiplier.
        /// </summary>
        /// <param name="multiplier">the desired multiplier.</param>
        public void SetScoreMultiplier(float multiplier)
        {
            _scoreMultiplier = multiplier;
        }
    }
}
