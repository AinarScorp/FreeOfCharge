
using UnityEngine;
using Player.Shooter;
namespace Inputs
{
    public class ColorController : MonoBehaviour
    {
        
        InputHandler _inputHandler;
        ColorPicker _colorPicker;
        void Awake()
        {
            _colorPicker = GetComponent<ColorPicker>();
            InputHandler[] inputHandlers = FindObjectsOfType<InputHandler>();
            foreach (var inputHandler in inputHandlers)
            {
                if (inputHandler.GetComponent<PlayerInfo>().Controls == PlayerControls.Shooter)
                {
                    _inputHandler = inputHandler;
                    break;
                }
            }
        }

        void OnEnable()
        {
            SetupControls(false);
        }
        
        void OnDisable()
        {
            DisableControls();
        
        }

        public void SetupControls(bool inverted)
        {
            _inputHandler.LeftButtonPressed +=  _colorPicker.SelectNextDelColor;
            _inputHandler.RightButtonPressed +=  _colorPicker.SelectNextDelShape;

            _inputHandler.DoubleButtonPressed += Shoot;
        }
        
        public void DisableControls()
        {
            _inputHandler.LeftButtonPressed -=  _colorPicker.SelectNextDelColor;
            _inputHandler.RightButtonPressed -=  _colorPicker.SelectNextDelShape;
            _inputHandler.DoubleButtonPressed -= Shoot;
        }
        

        void Shoot()
        {
            _colorPicker.ShootDelivery();
        }



    }
    
}
