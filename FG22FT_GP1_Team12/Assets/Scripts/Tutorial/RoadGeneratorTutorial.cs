using System.Collections;
using System.Collections.Generic;
using LevelDesign.Road;
using UnityEngine;

namespace Tutorial
{
    public class RoadGeneratorTutorial : RoadGenerator
    {
        [Header("Place a piece to repeat here")]
        [SerializeField] LevelPiece _pieceOfImportance;
        
        public override void UpdateLevelPiece(LevelPiece destoroyedLevelPiece)
        {
            if (_pieceOfImportance == null) return;
            LevelPiece newLevelPiece;
            if (destoroyedLevelPiece.gameObject == _pieceOfImportance.gameObject)
            {
                newLevelPiece = Instantiate(_pieceOfImportance, creatonPlace.position, Quaternion.identity, _roadSimulation.transform);
            }
            else
            {
                int randomNumber = Random.Range(0, _levelPieces.Length);
                newLevelPiece = Instantiate(_levelPieces[randomNumber], creatonPlace.position, Quaternion.identity, _roadSimulation.transform);
            }
            levelPiecesQueue.Enqueue(newLevelPiece);
            bottomPiece = levelPiecesQueue.Dequeue();

        }
    }
    
}
