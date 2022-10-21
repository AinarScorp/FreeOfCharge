using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelDesign.Road
{

    public class RoadGeneratorMain : RoadGenerator
    {
        // [SerializeField] RoadSimulation _roadSimulation;
        // [SerializeField] Transform creatonPlace, destructionPlace;
        // [SerializeField] LevelPiece[] startingPieces;
        // [SerializeField] float destructionDistance = 10;
        //
        // LevelPiece bottomPiece;
        // Queue<LevelPiece> levelPiecesQueue = new Queue<LevelPiece>();
        //
        //

        

        public override void UpdateLevelPiece(LevelPiece destoroyedLevelPiece)
        {
            if (_levelPieces.Length < 1) return;

            int randomNumber = Random.Range(0, _levelPieces.Length);
            LevelPiece newLevelPiece = Instantiate(_levelPieces[randomNumber], creationPlace, Quaternion.identity, _roadSimulation.transform);

            levelPiecesQueue.Enqueue(newLevelPiece);
            bottomPiece = levelPiecesQueue.Dequeue();

        }

        
    }
}