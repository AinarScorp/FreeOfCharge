using LevelDesign.Road;
using UnityEngine;

namespace Tutorial
{
    public class RoadGeneratorTutorial : RoadGenerator
    {
        [Header("Tutorial levels in order")]
        [SerializeField] LevelPiece[] _tutorialLevels;
        [Tooltip("Change this to set starting point for tutorial")]
        public TutorialSection CurrentTutorial;
        public LevelPiece CurrentTutorialLevel => _tutorialLevels[(int)CurrentTutorial];
        [SerializeField] int _emptyLevelsBetween;
        int _emptyLevelsToSpawn = 0;
        
        public override void UpdateLevelPiece(LevelPiece destroyedLevelPiece)
        {
            if (_levelPieces.Length < 1) return;

            LevelPiece newLevelPiece;
            if(_emptyLevelsToSpawn == 0 && (int)CurrentTutorial < _tutorialLevels.Length)
            {
                newLevelPiece = Instantiate(CurrentTutorialLevel, creationPlace, Quaternion.identity, _roadSimulation.transform);
            }
            else
            {
                int randomNumber = Random.Range(0, _levelPieces.Length);
                newLevelPiece = Instantiate(_levelPieces[randomNumber], creationPlace, Quaternion.identity, _roadSimulation.transform);
                _emptyLevelsToSpawn--;
            }
            levelPiecesQueue.Enqueue(newLevelPiece);
            bottomPiece = levelPiecesQueue.Dequeue();




        }

        /// <summary>
        /// Moves to the next tutorial section.
        /// </summary>
        public void NextSection()
        {
            CurrentTutorial += 1;
            _emptyLevelsToSpawn = _emptyLevelsBetween;
        }
    }
}
