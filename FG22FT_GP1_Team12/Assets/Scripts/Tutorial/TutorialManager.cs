using System.Collections;
using System.Collections.Generic;
using Player.Driver;
using UnityEngine;

namespace Tutorial
{



    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] ShipMovement playerPrefab;

        [Header("Steering tutorial")] [SerializeField]
        bool leftRightTutorialFinished;

        bool _movedLeft, _movedRight, _jump;

        public void TouchedTutorialCollider(TutorialColliderPoint point)
        {
            switch (point)
            {
                case TutorialColliderPoint.MoveLeft:
                    _movedLeft = true;
                    CheckMoveLeftRightTutorial();

                    break;
                case TutorialColliderPoint.MoveRight:
                    _movedRight = true;
                    CheckMoveLeftRightTutorial();
                    break;
            }
        }

        void CheckMoveLeftRightTutorial()
        {
            if (_movedLeft && _movedLeft)
            {
                leftRightTutorialFinished = true;
            }
        }
    }
}
