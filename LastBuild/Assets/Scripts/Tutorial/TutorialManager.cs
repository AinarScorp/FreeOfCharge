using Cinemachine;
using DeliveryZoneInfo;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{

    public enum TutorialSection
    {
        MoveLeft,
        MoveRight,
        ShootMiddle,
        ShootLeft,
        ShootRight,
        ShootLeft2,
        Jumping,
        JumpShooting,
        Colors,
        Colors2,
        Shapes,
        Shapes2,
        FinalLeft,
        FinalRight,
        FinalLeft2,
        End
    }


    public class TutorialManager : MonoBehaviour
    {
        public static event Action OnNextSection;

        [Header("References")]
        [SerializeField] GameObject _overHeadVisual;
        [SerializeField] Transform _inventoryContainer;
        [SerializeField] Image[] _crosshairImages;
        [SerializeField] RoadGeneratorTutorial _roadGenerator;
        [SerializeField] ShipMovementTutorial _playerMovement;
        [SerializeField] ColorInventoryUiTutorial _inventoryUi;
        [SerializeField] ColorPickerTutorial _colorPicker;

        [Header("Shooting Tutorial")]
        [SerializeField] int _shootingCountPerStage;
        [SerializeField] float _firstShotPauseTime;
        [SerializeField] float _slowdown;
        [SerializeField] float _inventoryAnimationSize;
        int _shootingCounter;

        [Header("Cameras")]
        [SerializeField] CinemachineVirtualCamera _mainCamera;
        [SerializeField] CinemachineVirtualCamera _pickingUpParticleCamera;
        [SerializeField] CinemachineVirtualCamera _changingColorCamera;

        [Header("Jumping Tutorial")]
        [SerializeField] int _jumpingCountPerStage;
        int _jumpingCounter;

        [Header("UI")]
        [SerializeField] Transform _moveLeftUi;
        [SerializeField] Transform _moveRightUi;
        [SerializeField] Transform _moveUi;

        [SerializeField] Transform _shootUi;

        [SerializeField] Transform _jumpUi;
        [SerializeField] Transform _jumpShootUi;

        [SerializeField] Transform _colorUi;
        [SerializeField] Transform _shapeUi;

        void OnEnable()
        {
            DeliveryTutorial.OnCompletedDelivery += SucessfulDelivery;
        }

        void OnDisable()
        {
            DeliveryTutorial.OnCompletedDelivery -= SucessfulDelivery;
        }

        #region Generic Tutorial Methods

        void DisableTutorialUi()
        {
            _moveLeftUi.gameObject.SetActive(false);
            _moveRightUi.gameObject.SetActive(false);
            _moveUi.gameObject.SetActive(false);
            _shootUi.gameObject.SetActive(false);
            _jumpUi.gameObject.SetActive(false);
            _jumpShootUi.gameObject.SetActive(false);
            _colorUi.gameObject.SetActive(false);
            _shapeUi.gameObject.SetActive(false);
        }

        /// <summary>
        /// Initializes the current stage of the tutorial.
        /// </summary>
        public void InitTutorial()
        {
            switch(_roadGenerator.CurrentTutorial)
            {
                case TutorialSection.MoveLeft:
                    InitMovementTutorial();
                    break;

                case TutorialSection.ShootMiddle:
                    InitShootingTutorial();
                    break;

                case TutorialSection.Jumping:
                    InitJumpingTutorial();
                    break;

                case TutorialSection.JumpShooting:
                    InitJumpShootingTutorial();
                    break;

                case TutorialSection.Colors:
                    InitColorsTutorial();
                    break;

                case TutorialSection.Colors2:
                    InitColors2Tutorial();
                    break;

                case TutorialSection.Shapes:
                    InitShapesTutorial();
                    break;

                case TutorialSection.Shapes2:
                    InitShapes2Tutorial();
                    break;

                case TutorialSection.FinalLeft:
                    InitFinalTutorial();
                    break;

                case TutorialSection.End:
                    Application.Quit();
                    break;
            }
        }

        /// <summary>
        /// Finishes the current section and moves to the next.
        /// </summary>
        /// <param name="section">the section that was finished.</param>
        public void SectionDone(TutorialSection section)
        {
            NextSection();
            switch(section)
            {
                case TutorialSection.MoveLeft:
                    DisableTutorialUi();
                    _moveUi.gameObject.SetActive(true);
                    _moveRightUi.gameObject.SetActive(true);
                    break;

                case TutorialSection.MoveRight:
                    DisableTutorialUi();
                    _moveUi.gameObject.SetActive(true);
                    _moveLeftUi.gameObject.SetActive(true);
                    break;

                case TutorialSection.ShootMiddle:
                    _playerMovement.CanMove = true;
                    _shootingCountPerStage = 1;
                    _shootingCounter = 1;
                    return;
            }
            InitTutorial();
        }

        /// <summary>
        /// Moves to the next section.
        /// </summary>
        void NextSection()
        {
            _roadGenerator.NextSection();
            OnNextSection?.Invoke();
        }

        /// <summary>
        /// Moves the player to the center of the screen.
        /// </summary>
        IEnumerator MoveToCenter()
        {
            var lerp = 0f;
            var originalPos = _playerMovement.transform.position;

            _playerMovement.enabled = false;

            while (lerp < 1)
            {
                lerp += Time.deltaTime;
                var targetPos = new Vector3(0, _playerMovement.transform.position.y, _playerMovement.transform.position.z);
                _playerMovement.transform.position = Vector3.Lerp(originalPos, targetPos, lerp);
                yield return null;
            }
            _playerMovement.enabled = true;
        }

        /// <summary>
        /// Finishes section if player has hit the right amount of delivery points.
        /// </summary>
        void SucessfulDelivery()
        {
            _shootingCounter -= 1;
            if (_shootingCounter == 0)
            {
                SectionDone(_roadGenerator.CurrentTutorial);
                _shootingCounter = _shootingCountPerStage;
            }
        }

        #endregion

        #region Movement Tutorial

        /// <summary>
        /// Initializes the movement tutorial.
        /// </summary>
        void InitMovementTutorial()
        {
            _crosshairImages[0].enabled = false;
            _crosshairImages[1].enabled = false;

            _inventoryUi.ColorCount(0);
            _inventoryUi.ShapeCount(0);

            _colorPicker.ColorLocked = true;
            _colorPicker.ShapeLocked = true;
            _colorPicker.ResetSelection();
            _colorPicker.CanShoot = false;

            _overHeadVisual.SetActive(false);

            _playerMovement.JumpLocked = true;
            _playerMovement.CanMove = true;

            DisableTutorialUi();
            _moveLeftUi.gameObject.SetActive(true);
            _moveUi.gameObject.SetActive(true);
        }

        #endregion

        #region Shooting Tutorial

        /// <summary>
        /// Starts the first shot sequence.
        /// </summary>
        void FirstShot()
        {
            ColorPickerTutorial.OnShootDelivery -= FirstShot;
            DeliveryTutorial.OnCompletedDelivery += FirstDelivery;
            StartCoroutine(FirstShotSequence());
        }

        /// <summary>
        /// Starts the first delivery sequence.
        /// </summary>
        void FirstDelivery()
        {
            DeliveryTutorial.OnCompletedDelivery -= FirstDelivery;

            StartCoroutine(FirstDeliverySequence());
        }

        /// <summary>
        /// Initializes the shooting tutorial.
        /// </summary>
        void InitShootingTutorial()
        {
            _playerMovement.CanMove = false;

            _inventoryUi.ColorCount(1);
            _inventoryUi.ShapeCount(1);

            _colorPicker.ColorLocked = true;
            _colorPicker.ShapeLocked = true;
            _colorPicker.CanShoot = true;
            _colorPicker.ResetSelection();

            _crosshairImages[0].enabled = true;
            _crosshairImages[1].enabled = true;

            _overHeadVisual.SetActive(true);

            DeliveryTutorial.AllowedColors = new DeliverableColor[] { DeliverableColor.Red };
            DeliveryTutorial.AllowedShapes = new DeliverableShape[] { DeliverableShape.Emerald };

            ColorPickerTutorial.OnShootDelivery += FirstShot;

            _shootingCounter = _shootingCountPerStage;

            DisableTutorialUi();
            _shootUi.gameObject.SetActive(true);

            StartCoroutine(MoveToCenter());
        }

        /// <summary>
        /// Starts the sequence of events when the player hits the first delivery point.
        /// </summary>
        IEnumerator FirstDeliverySequence()
        {
            _mainCamera.Priority = 0;
            _pickingUpParticleCamera.Priority = 100;

            yield return new WaitForSecondsRealtime(3);

            _mainCamera.Priority = 100;
            _pickingUpParticleCamera.Priority = 0;

            var lerp = 0f;
            while (lerp < 1)
            {

                _inventoryContainer.transform.localScale = new Vector3(1, 1, 1) * (Mathf.Lerp(_inventoryAnimationSize, 1, lerp));

                lerp += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Starts the sequence of events when the player fires the first shot.
        /// </summary>
        IEnumerator FirstShotSequence()
        {
            yield return new WaitForSeconds(.05f);
            var originalTimescale = Time.timeScale;
            Time.timeScale = _slowdown;
            yield return InventorySizeAnimation();
            Time.timeScale = originalTimescale;
        }

        /// <summary>
        /// Starts inventory size increase animation.
        /// </summary>
        IEnumerator InventorySizeAnimation()
        {
            var lerp = 0f;
            while (lerp < 1)
            {

                _inventoryContainer.transform.localScale = new Vector3(1, 1, 1) * (Mathf.Lerp(1, _inventoryAnimationSize, lerp));

                lerp += Time.unscaledDeltaTime;
                yield return null;
            }

            yield return new WaitForSecondsRealtime(_firstShotPauseTime);
        }

        #endregion

        #region Jumping Tutorial

        /// <summary>
        /// Initializes the jumping tutorial.
        /// </summary>
        void InitJumpingTutorial()
        {
            _crosshairImages[0].enabled = false;
            _crosshairImages[1].enabled = false;

            _inventoryUi.ColorCount(0);
            _inventoryUi.ShapeCount(0);

            _colorPicker.ColorLocked = true;
            _colorPicker.ShapeLocked = true;
            _colorPicker.ResetSelection();
            _colorPicker.CanShoot = false;

            _overHeadVisual.SetActive(false);

            _playerMovement.JumpLocked = false;
            _playerMovement.CanMove = true;

            TutorialObstacleClear.OnObstacleClear += ObstacleCleared;

            _jumpingCounter = _jumpingCountPerStage;

            DisableTutorialUi();
            _jumpUi.gameObject.SetActive(true);

            StartCoroutine(MoveToCenter());
        }

        /// <summary>
        /// Finishes jumping section if the required amount of obstacles have been cleared.
        /// </summary>
        void ObstacleCleared()
        {
            _jumpingCounter--;
            if (_jumpingCounter == 0)
            {
                SectionDone(_roadGenerator.CurrentTutorial);
            }
        }

        #endregion

        #region Jump Shooting Tutorial

        /// <summary>
        /// Initializes the jump shooting tutorial.
        /// </summary>
        void InitJumpShootingTutorial()
        {
            _playerMovement.CanMove = true;
            _playerMovement.JumpLocked = false;
            
            _inventoryUi.ColorCount(1);
            _inventoryUi.ShapeCount(1);

            _colorPicker.ColorsActive = 1;
            _colorPicker.ShapesActive = 1;
            
            _colorPicker.ColorLocked = true;
            _colorPicker.ShapeLocked = true;
            _colorPicker.CanShoot = true;
            _colorPicker.ResetSelection();
            
            _crosshairImages[0].enabled = true;
            _crosshairImages[1].enabled = true;
            
            _overHeadVisual.SetActive(true);

            DeliveryTutorial.AllowedColors = new DeliverableColor[] { DeliverableColor.Red };
            DeliveryTutorial.AllowedShapes = new DeliverableShape[] { DeliverableShape.Emerald };

            _shootingCountPerStage = 3;
            _shootingCounter = _shootingCountPerStage;

            DisableTutorialUi();
            _jumpShootUi.gameObject.SetActive(true);

            StartCoroutine(MoveToCenter());
        }

        #endregion

        #region Color Swap Tutorial

        /// <summary>
        /// Initializes the color swapping tutorial.
        /// </summary>
        void InitColorsTutorial()
        {
            DeliveryTutorial.AllowedColors = new DeliverableColor[] { DeliverableColor.Blue };
            DeliveryTutorial.AllowedShapes = new DeliverableShape[] { DeliverableShape.Emerald }; 
            
            _playerMovement.CanMove = true;
            _playerMovement.JumpLocked = true;

            _colorPicker.ColorLocked = false;
            _colorPicker.ShapeLocked = false;
            _colorPicker.CanShoot = false;

            _inventoryUi.ColorCount(1);
            _inventoryUi.ShapeCount(1);

            _colorPicker.ColorsActive = 1;
            _colorPicker.ShapesActive = 1;
            _colorPicker.ResetSelection();

            _crosshairImages[0].enabled = true;
            _crosshairImages[1].enabled = true;
            _inventoryUi.UpdateCrosshairs(_colorPicker.CurrentDeliveryColor, _colorPicker.CurrentDeliveryShape);

            _overHeadVisual.SetActive(true);

            _shootingCounter = _shootingCountPerStage;

            DisableTutorialUi();
            _colorUi.gameObject.SetActive(true);

            StartCoroutine(ColorSwapSequence());

        }

        /// <summary>
        /// Starts the sequence of events when the player should swap their colors for the first time.
        /// </summary>
        IEnumerator ColorSwapSequence()
        {
            yield return new WaitForSeconds(2);
            yield return StartCoroutine(MoveToCenter());
            _playerMovement.CanMove = false;
            yield return new WaitForSeconds(4);

            _colorPicker.CanShoot = true;

            _inventoryUi.ColorCount(2);
            _inventoryUi.ShapeCount(1);

            _colorPicker.ColorsActive = 2;
            _colorPicker.ShapesActive = 1;

            ColorPickerTutorial.OnShootDelivery += ColorSwapFirstShot;
            _mainCamera.Priority = 0;
            _changingColorCamera.Priority = 100;

            Time.timeScale = 0;
        }

        /// <summary>
        /// Initializes the second part of the color swap tutorial.
        /// </summary>
        void InitColors2Tutorial()
        {
            DeliveryTutorial.AllowedColors = new DeliverableColor[] { DeliverableColor.Yellow };
            DeliveryTutorial.AllowedShapes = new DeliverableShape[] { DeliverableShape.Emerald };

            _playerMovement.CanMove = false;
            _playerMovement.JumpLocked = true;

            _colorPicker.ColorLocked = false;
            _colorPicker.ShapeLocked = false;
            _colorPicker.CanShoot = false;

            _inventoryUi.ColorCount(2);
            _inventoryUi.ShapeCount(1);

            _colorPicker.ColorsActive = 2;
            _colorPicker.ShapesActive = 1;

            _crosshairImages[0].enabled = true;
            _crosshairImages[1].enabled = true;
            _inventoryUi.UpdateCrosshairs(_colorPicker.CurrentDeliveryColor, _colorPicker.CurrentDeliveryShape);

            _overHeadVisual.SetActive(true);

            _shootingCounter = _shootingCountPerStage;

            StartCoroutine(ColorSwap2Sequence());
        }

        /// <summary>
        /// Starts the sequence of events when the player should swap their colors for the second time.
        /// </summary>
        IEnumerator ColorSwap2Sequence()
        {
            yield return new WaitForSeconds(6);

            _colorPicker.CanShoot = true;

            _inventoryUi.ColorCount(3);
            _inventoryUi.ShapeCount(1);

            _colorPicker.ColorsActive = 3;
            _colorPicker.ShapesActive = 1;

            ColorPickerTutorial.OnShootDelivery += ColorSwapFirstShot;

            //Time.timeScale = 0;
        }

        /// <summary>
        /// Resets the timescale when the player fires their first shot after swapping colors.
        /// </summary>
        void ColorSwapFirstShot()
        {
            _mainCamera.Priority = 100;
            _changingColorCamera.Priority = 0;
            Time.timeScale = 1;
            ColorPickerTutorial.OnShootDelivery -= ColorSwapFirstShot;
        }

        #endregion

        #region Shape Swap Tutorial

        /// <summary>
        /// Initializes the shape swap tutorial.
        /// </summary>
        void InitShapesTutorial()
        {
            DeliveryTutorial.AllowedColors = new DeliverableColor[] { DeliverableColor.Red };
            DeliveryTutorial.AllowedShapes = new DeliverableShape[] { DeliverableShape.Heart };

            _playerMovement.CanMove = false;
            _playerMovement.JumpLocked = true;

            _colorPicker.ColorLocked = false;
            _colorPicker.ShapeLocked = false;
            _colorPicker.CanShoot = false;

            _inventoryUi.ColorCount(3);
            _inventoryUi.ShapeCount(1);

            _colorPicker.ColorsActive = 3;
            _colorPicker.ShapesActive = 1;

            _crosshairImages[0].enabled = true;
            _crosshairImages[1].enabled = true;
            _inventoryUi.UpdateCrosshairs(_colorPicker.CurrentDeliveryColor, _colorPicker.CurrentDeliveryShape);

            _overHeadVisual.SetActive(true);

            _shootingCounter = _shootingCountPerStage;

            StartCoroutine(ShapeSwapSequence());

            StartCoroutine(MoveToCenter());

            DisableTutorialUi();
            _shapeUi.gameObject.SetActive(true);
        }

        /// <summary>
        /// Starts the sequence of events when the player should swap their shapes for the first time.
        /// </summary>
        IEnumerator ShapeSwapSequence()
        {
            yield return new WaitForSeconds(7);

            _colorPicker.CanShoot = true;

            _inventoryUi.ColorCount(3);
            _inventoryUi.ShapeCount(2);

            _colorPicker.ColorsActive = 3;
            _colorPicker.ShapesActive = 2;

            ColorPickerTutorial.OnShootDelivery += ColorSwapFirstShot;

            Time.timeScale = 0;

            _mainCamera.Priority = 0;
            _changingColorCamera.Priority = 100;
        }

        /// <summary>
        /// Initializes the second part of the shape swap tutorial.
        /// </summary>
        void InitShapes2Tutorial()
        {
            DeliveryTutorial.AllowedColors = new DeliverableColor[] { DeliverableColor.Red };
            DeliveryTutorial.AllowedShapes = new DeliverableShape[] { DeliverableShape.Star };

            _playerMovement.CanMove = false;
            _playerMovement.JumpLocked = true;

            _colorPicker.ColorLocked = false;
            _colorPicker.ShapeLocked = false;
            _colorPicker.CanShoot = false;

            _inventoryUi.ColorCount(3);
            _inventoryUi.ShapeCount(2);

            _colorPicker.ColorsActive = 3;
            _colorPicker.ShapesActive = 2;

            _crosshairImages[0].enabled = true;
            _crosshairImages[1].enabled = true;
            _inventoryUi.UpdateCrosshairs(_colorPicker.CurrentDeliveryColor, _colorPicker.CurrentDeliveryShape);

            _overHeadVisual.SetActive(true);

            _shootingCounter = _shootingCountPerStage;

            StartCoroutine(ShapeSwap2Sequence());
        }

        /// <summary>
        /// Starts the sequence of events when the player should swap their shapes for the second time.
        /// </summary>
        /// <returns></returns>
        IEnumerator ShapeSwap2Sequence()
        {
            yield return new WaitForSeconds(5);

            _colorPicker.CanShoot = true;

            _inventoryUi.ColorCount(3);
            _inventoryUi.ShapeCount(3);

            _colorPicker.ColorsActive = 3;
            _colorPicker.ShapesActive = 3;

            ColorPickerTutorial.OnShootDelivery += ColorSwapFirstShot;

            Time.timeScale = 0;
        }

        #endregion

        #region Final Tutorial

        /// <summary>
        /// Initializes the final tutorial.
        /// </summary>
        void InitFinalTutorial()
        {
            DeliveryTutorial.AllowedColors = new DeliverableColor[] {DeliverableColor.Red, DeliverableColor.Blue, DeliverableColor.Yellow };
            DeliveryTutorial.AllowedShapes = new DeliverableShape[] { DeliverableShape.Emerald, DeliverableShape.Heart, DeliverableShape.Star };

            _playerMovement.CanMove = true;
            _playerMovement.JumpLocked = false;

            _colorPicker.CanShoot = true;
            _colorPicker.ColorsActive = 3;
            _colorPicker.ShapesActive = 3;
            _colorPicker.ColorLocked = false;
            _colorPicker.ShapeLocked = false;

            _crosshairImages[0].enabled = true;
            _crosshairImages[1].enabled = true;

            _inventoryUi.UpdateCrosshairs(_colorPicker.CurrentDeliveryColor, _colorPicker.CurrentDeliveryShape);
            _inventoryUi.ColorCount(3);
            _inventoryUi.ShapeCount(3);

            _overHeadVisual.SetActive(true);

            _shootingCountPerStage = 3;
            _shootingCounter = 3;
            DisableTutorialUi();
        }

        #endregion
    }
}
