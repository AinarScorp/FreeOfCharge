using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player.Shooter;
using DeliveryZoneInfo;

namespace Ui.DeliverInfo
{
    public class ColorInventoryUi : MonoBehaviour
    {
        
        [SerializeField] HorizontalLayoutGroup colorTransform, shapeTransform, colorAmmoTransform,shapeAmmoTransform;
        [SerializeField] Image colorImagePrefab;
        [SerializeField] Image shapeImagePrefab;
        [SerializeField] Image colorCrosshair, shapeCrosshair;
        [SerializeField] Image ammoImage;
        [SerializeField] ColorPicker _colorPicker;
        [SerializeField] Sprite starImage, heartImage, emeraldImage;
        protected List<Image> _colorImages = new List<Image>();
        protected List<Image> _shapeImages = new List<Image>();


        
        void OnEnable()
        {
            //ColorPicker.OnInventoryChange.AddListener(UpdateInventory);
            _colorPicker.NewColorSelected += UpdateColorCrosshair;
            _colorPicker.NewShapeSelected += UpdateShapeCrosshair;

        }
        
        
        void OnDisable()
        {
            //ColorPicker.OnInventoryChange.RemoveListener(UpdateInventory);
            _colorPicker.NewColorSelected -= UpdateColorCrosshair;
            _colorPicker.NewShapeSelected -= UpdateShapeCrosshair;
        }

        protected void UpdateColorCrosshair(DeliveryContainer<DeliverableColor> color)
        {
            colorCrosshair.rectTransform.position = _colorImages[_colorPicker.DeliverableColors.IndexOf(color)].rectTransform.position;
        }
        protected void UpdateShapeCrosshair(DeliveryContainer<DeliverableShape> shape)
        {
            shapeCrosshair.rectTransform.position = _shapeImages[_colorPicker.DeliverableShapes.IndexOf(shape)].rectTransform.position;
        }
        protected virtual void Awake()
        {
            foreach (var color in _colorPicker.DeliverableColors)
            {
                Image createdImage = Instantiate(colorImagePrefab, colorTransform.transform);
                Image createdBatter = Instantiate(ammoImage, colorAmmoTransform.transform);
                CountDownImage countDownImage = createdBatter.GetComponentInChildren<CountDownImage>();
                countDownImage.SetupCountDown(color);
                color.ChargeModified+= countDownImage.UpdateCountDown;


                createdImage.color = UpdateSLotColor(color.GetContainerType());
                _colorImages.Add(createdImage) ;
            }

            foreach (var shape in _colorPicker.DeliverableShapes)
            {
                Image createdImage = Instantiate(shapeImagePrefab, shapeTransform.transform);
                Image createdBatter = Instantiate(ammoImage, shapeAmmoTransform.transform);

                CountDownImage countDownImage = createdBatter.GetComponentInChildren<CountDownImage>();
                countDownImage.SetupCountDown(shape);
                shape.ChargeModified+= countDownImage.UpdateCountDown;

                //CountDownImage<DeliveryContainer<DeliverableShape>> countDownImage = createdImage.GetComponentInChildren<>()
                createdImage.color = UpdateSlotShape(shape.GetContainerType());
                createdImage.sprite = UpdateSlotShapeSprite(shape.GetContainerType());
                _shapeImages.Add(createdImage) ;
                
            }
        }



        Color UpdateSlotShape(DeliverableShape shape)
        {
            switch (shape)
            {
                case DeliverableShape.Emerald:
                    return Color.green;

                case DeliverableShape.Heart:
                    return Color.magenta;

                case DeliverableShape.Star:
                    return Color.yellow;
            }

            return Color.white;
        }
        Sprite UpdateSlotShapeSprite(DeliverableShape shape)
        {
            switch (shape)
            {
                case DeliverableShape.Emerald:
                    return emeraldImage;

                case DeliverableShape.Heart:
                    return heartImage;

                case DeliverableShape.Star:
                    return starImage;
            }

            return null;
        }
        Color UpdateSLotColor(DeliverableColor color)
        {
            switch (color)
                {
                    case DeliverableColor.Red:
                        return Color.red;

                    case DeliverableColor.Blue:
                        return Color.blue;

                    case DeliverableColor.Yellow:
                        return Color.yellow;
                }
            return Color.white;

        }
    }
}
