using System;
using System.Collections;
using System.Collections.Generic;
using Einar.Core;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] LevelPiece[] _levelPieces;
    [SerializeField] RoadSimulation _roadSimulation;
    [SerializeField] Transform creatonPlace, destructionPlace;
    [SerializeField] LevelPiece[] startingPieces;
    [SerializeField] float destructionDistance = 10;
    int currentLevel = 1;
    
    LevelPiece bottomPiece;
    Queue<LevelPiece> levelPiecesQueue = new Queue<LevelPiece>();
    void Awake()
    {
        if (_roadSimulation == null)
        {
            _roadSimulation = FindObjectOfType<RoadSimulation>();
        }
    }

    void Start()
    {
        foreach (var piece in startingPieces)
        {
            levelPiecesQueue.Enqueue(piece);
            
        }

        bottomPiece = levelPiecesQueue.Dequeue();
    }

    void Update()
    {
        if (bottomPiece == null) return;
        float distanceToDestruction = Vector3.Distance(bottomPiece.transform.position, destructionPlace.position);
        if (distanceToDestruction<= destructionDistance)
        {
            UpdateLevelPiece();
        }
    }

    void UpdateLevelPiece()
    {
        if (_levelPieces.Length < 1) return;
        
        int randomNumber = Random.Range(0, _levelPieces.Length);
        LevelPiece newLevelPiece = Instantiate(_levelPieces[randomNumber], creatonPlace.position, Quaternion.identity, _roadSimulation.transform);
        
        levelPiecesQueue.Enqueue(newLevelPiece);
        bottomPiece = levelPiecesQueue.Dequeue();
        
    }

    void IncreaseLevel()
    {
        currentLevel++;
    }

    void DecreaseLevel()
    {
        currentLevel--;
    }

    void OnDrawGizmos()
    {
        
        if (creatonPlace ==null || destructionPlace == null) return;
        
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(creatonPlace.position,Vector3.one);
        Gizmos.DrawCube(destructionPlace.position,Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(destructionPlace.position, destructionDistance);
    }
}