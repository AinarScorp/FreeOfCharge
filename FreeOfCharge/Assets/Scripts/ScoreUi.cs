using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace William
{
    public class ScoreUi : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _scoreText;

        void OnEnable()
        {
            ScoreController.OnScoreAdded += UpdateScoreCounter;
            ScoreController.OnScoreRemoved += UpdateScoreCounter;
        }

        void OnDisable()
        {
            ScoreController.OnScoreAdded += UpdateScoreCounter;
            ScoreController.OnScoreRemoved += UpdateScoreCounter;
        }

        /// <summary>
        /// Updates the score counter
        /// </summary>
        /// <param name="amount">the score amount.</param>
        void UpdateScoreCounter(int amount)
        {
            _scoreText.text = amount.ToString();
        }
    }
}
