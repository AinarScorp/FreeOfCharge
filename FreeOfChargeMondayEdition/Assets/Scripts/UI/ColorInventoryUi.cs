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
        [SerializeField] HorizontalLayoutGroup colorTransform, shapeTransform;
        [SerializeField] Image colorImagePrefab;
        [SerializeField] Image shapeImagePrefab;
        [SerializeField] Image colorCrosshair, shapeCrosshair;

        [SerializeField] ColorPicker _colorPicker;
        [SerializeField] Sprite starImage, heartImage, emeraldImage;
        List<Image> colorImages = new List<Image>();
        List<Image> shapeImages = new List<Image>();



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

        void UpdateColorCrosshair(DeliveryContainer<DeliverableColor> color)
        {
            colorCrosshair.rectTransform.position = colorImages[_colorPicker.DeliverableColors.IndexOf(color)].rectTransform.position;
        }
        void UpdateShapeCrosshair(DeliveryContainer<DeliverableShape> shape)
        {
            shapeCrosshair.rectTransform.position = shapeImages[_colorPicker.DeliverableShapes.IndexOf(shape)].rectTransform.position;
        }
        void Start()
        {
            foreach (var color in _colorPicker.DeliverableColors)
            {
                Image createdImage = Instantiate(colorImagePrefab, colorTransform.transform);
                CountDownImage countDownImage = createdImage.GetComponentInChildren<CountDownImage>();
                countDownImage.SetupCountDown(color);
                color.ChargeAdded += countDownImage.UpdateCountDown;
                color.ChargeSubtracted += countDownImage.UpdateCountDown;

                createdImage.color = UpdateSLotColor(color.GetType());
                colorImages.Add(createdImage) ;
            }

            foreach (var shape in _colorPicker.DeliverableShapes)
            {
                Image createdImage = Instantiate(shapeImagePrefab, shapeTransform.transform);
                CountDownImage countDownImage = createdImage.GetComponentInChildren<CountDownImage>();
                countDownImage.SetupCountDown(shape);
                shape.ChargeAdded += countDownImage.UpdateCountDown;
                shape.ChargeSubtracted += countDownImage.UpdateCountDown;
                //CountDownImage<DeliveryContainer<DeliverableShape>> countDownImage = createdImage.GetComponentInChildren<>()
                createdImage.color = UpdateSlotShape(shape.GetType());
                createdImage.sprite = UpdateSlotShapeSprite(shape.GetType());
                shapeImages.Add(createdImage) ;
                
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
