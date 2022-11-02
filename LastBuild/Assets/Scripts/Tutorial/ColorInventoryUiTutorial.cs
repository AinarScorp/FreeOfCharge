using DeliveryZoneInfo;
using System.Collections.Generic;
using Ui.DeliverInfo;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class ColorInventoryUiTutorial : ColorInventoryUi
    {
        public List<Image> ShapeImages => _shapeImages;
        public List<Image> ColorImages => _colorImages;
        [Header("Tutorial Stuff")]
        [SerializeField] Transform _colorAmmoParent;
        [SerializeField] Transform _shapeAmmoParent;
        [SerializeField] TutorialManager _tutorialManager;

        protected override void Awake()
        {
            base.Awake();
            _tutorialManager.InitTutorial();
        }

        /// <summary>
        /// Sets the amount of colors the ui should show.
        /// </summary>
        /// <param name="amount">the amount of colors.</param>
        public void ColorCount(int amount)
        {
            var colorAmmo = new List<Image>();
            foreach (Transform child in _colorAmmoParent.transform)
            {
                var image = child.GetComponent<Image>();
                colorAmmo.Add(image);
                image.enabled = false;
                image.transform.GetChild(0).gameObject.SetActive(false);
            }

            foreach (Image color in _colorImages)
            {
                color.enabled = false;
            }

            for (int i = 0; i < amount; i++)
            {
                _colorImages[i].enabled = true;
                colorAmmo[i].enabled = true;
                colorAmmo[i].GetComponentInChildren<Image>().enabled = true;
                colorAmmo[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Sets the amount of shapes the ui should show.
        /// </summary>
        /// <param name="amount">the amount of shapes.</param>
        public void ShapeCount(int amount)
        {
            var shapeAmmo = new List<Image>();
            foreach (Transform child in _shapeAmmoParent.transform)
            {
                var image = child.GetComponent<Image>();
                shapeAmmo.Add(image);
                image.enabled = false;
                image.transform.GetChild(0).gameObject.SetActive(false);
            }

            foreach (Image shape in _shapeImages)
            {
                shape.enabled = false;
            }

            for (int i = 0; i < amount; i++)
            {
                _shapeImages[i].enabled = true;
                shapeAmmo[i].enabled = true;
                shapeAmmo[i].GetComponentInChildren<Image>().enabled = true;
                shapeAmmo[i].transform.GetChild(0).gameObject.SetActive(true);

            }
        }

        /// <summary>
        /// Updates both the color and shape crosshairs.
        /// </summary>
        /// <param name="color">the color the crosshair should be on.</param>
        /// <param name="shape">the shape the crosshair should be on.</param>
        public void UpdateCrosshairs(DeliveryContainer<DeliverableColor> color, DeliveryContainer<DeliverableShape> shape)
        {
            UpdateColorCrosshair(color);
            UpdateShapeCrosshair(shape);
        }
    }
}