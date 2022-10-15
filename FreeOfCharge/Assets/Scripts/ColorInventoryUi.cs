using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace William
{
    public class ColorInventoryUi : MonoBehaviour
    {
        [SerializeField] HorizontalLayoutGroup colorTransform, shapeTransform;
        [SerializeField] Image colorImagePrefab;
        [SerializeField] Image shapeImagePrefab;
        [SerializeField] Image colorCrosshair, shapeCrosshair;

        [SerializeField] ColorPicker _colorPicker;
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

        void UpdateColorCrosshair(DeliverableColor color)
        {
            colorCrosshair.rectTransform.position = colorImages[_colorPicker.DeliverableColors.IndexOf(color)].rectTransform.position;
        }
        void UpdateShapeCrosshair(DeliverableShape shape)
        {
            shapeCrosshair.rectTransform.position = shapeImages[_colorPicker.DeliverableShapes.IndexOf(shape)].rectTransform.position;
        }
        void Start()
        {
            foreach (var color in _colorPicker.DeliverableColors)
            {
                Image createdImage = Instantiate(colorImagePrefab, colorTransform.transform);

                createdImage.color = UpdateSLotColor(color);
                colorImages.Add(createdImage) ;
            }

            foreach (var shape in _colorPicker.DeliverableShapes)
            {
                Image cretedImage = Instantiate(shapeImagePrefab, shapeTransform.transform);
                cretedImage.color = UpdateSlotShape(shape);
                shapeImages.Add(cretedImage) ;
                
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