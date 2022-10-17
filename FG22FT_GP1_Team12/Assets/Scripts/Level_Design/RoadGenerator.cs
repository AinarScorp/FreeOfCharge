using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelDesign.Road
{
    public abstract class RoadGenerator : MonoBehaviour
    {

        //[SerializeField] LevelPiece[] _levelPieces;
        [Header("Place pieces that already exist on the map here\nMIND THE ORDER")]
        [SerializeField] protected LevelPiece[] startingPieces;
        [Header("Place pieces that you want to scroll randomly\nORDER DOESN'T MATTER")]
        [SerializeField] protected LevelPiece[] _levelPieces;

        [Header("Setup spawning")]
        [SerializeField] float destructionDistance = 10;
        [SerializeField] protected Transform creatonPlace, destructionPlace;

        protected RoadSimulation _roadSimulation;
        protected LevelPiece bottomPiece;
        protected Queue<LevelPiece> levelPiecesQueue = new Queue<LevelPiece>();

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
            if (distanceToDestruction <= destructionDistance)
            {
                UpdateLevelPiece(bottomPiece);
            }
        }

        public virtual void UpdateLevelPiece(LevelPiece destoroyedLevelPiece)
        {
            //if (_levelPieces.Length < 1) return;
            //
            // int randomNumber = Random.Range(0, _levelPieces.Length);
            // LevelPiece newLevelPiece = Instantiate(_levelPieces[randomNumber], creatonPlace.position, Quaternion.identity, _roadSimulation.transform);
            //
            // levelPiecesQueue.Enqueue(newLevelPiece);
            // bottomPiece = levelPiecesQueue.Dequeue();

        }


        void OnDrawGizmos()
        {

            if (creatonPlace == null || destructionPlace == null) return;


            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(creatonPlace.position, Vector3.one);
            Gizmos.DrawCube(destructionPlace.position, Vector3.one);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(destructionPlace.position, destructionDistance);
        }
    }
}