using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelDesign.Road
{
    
    
public class RoadGenerator : MonoBehaviour
{

    //[SerializeField] LevelPiece[] _levelPieces;
        [Header("Place pieces that already exist on the map here\nMIND THE ORDER")]
        [SerializeField] protected LevelPiece[] startingPieces;
        [Header("Place pieces that you want to scroll randomly\nORDER DOESN'T MATTER")]
        [SerializeField] protected LevelPiece[] _levelPieces;

        [Header("Setup spawning")]
        [SerializeField] float destructionDistance = 10;
        protected Transform creatonPlace;
        [SerializeField] protected Transform destructionPlace;
        [SerializeField] protected Vector3 creationPlace = Vector3.zero;

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
            
            creationPlace = (startingPieces[1].transform.position - startingPieces[0].transform.position) * (startingPieces.Length-1 );
            
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


            Gizmos.color = Color.green;
            Gizmos.DrawCube(creationPlace, Vector3.one);
            Gizmos.color = Color.magenta;

            Gizmos.DrawCube(destructionPlace.position, Vector3.one);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(destructionPlace.position, destructionDistance);
        }
}

}
