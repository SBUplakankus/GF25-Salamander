// ============================================================================================
// CLASS: AttributeSliderController
// ============================================================================================
// Description:
//   Controls the three primary attribute radial sliders in the top left of the screen
//
// Methods:
//   - HandleInitSliderValues: Sets the initial slider values at the start of the game
//   - HandleHealthUpdate: Updates the health sliders value
//   - HandleHungerUpdate: Updates the hunger sliders value
//   - HandleMoistUpdate: Updates the moist sliders value
//
// Related Classes:
//   - PlayerAttributes: Listens in on attribute value change events
// ============================================================================================

using System;
using Player;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AttributeSliderController : MonoBehaviour
    {
        [Header("Attribute Sliders")] 
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider hungerSlider;
        [SerializeField] private Slider moistSlider;

        [Header("Animation Values")] 
        private const float AnimationDuration = 0.5f;
        private const Ease AnimationEase = Ease.OutCubic;

        private void OnEnable()
        {
            PlayerAttributes.OnHealthLevelChanged += HandleHealthUpdate;
            PlayerAttributes.OnHungerLevelChanged += HandleHungerUpdate;
            PlayerAttributes.OnMoistLevelChanged += HandleMoistUpdate;
            PlayerAttributes.OnInitSliderValues += HandleInitSliderValues;
        }
        
        private void OnDisable()
        {
            PlayerAttributes.OnHealthLevelChanged -= HandleHealthUpdate;
            PlayerAttributes.OnHungerLevelChanged -= HandleHungerUpdate;
            PlayerAttributes.OnMoistLevelChanged -= HandleMoistUpdate;
            PlayerAttributes.OnInitSliderValues -= HandleInitSliderValues;
        }

        private void HandleInitSliderValues(int healthMax, int hungerMax, int moistMax)
        {
            healthSlider.maxValue = healthMax;
            hungerSlider.maxValue = hungerMax;
            moistSlider.maxValue = moistMax;
            healthSlider.value = healthMax;
            moistSlider.value = moistMax;
            hungerSlider.value = hungerMax;
        }
        private void HandleHealthUpdate(int value)
        {
            Tween.UISliderValue(healthSlider, value, AnimationDuration, AnimationEase);
        }
        
        private void HandleHungerUpdate(int value)
        {
            Tween.UISliderValue(hungerSlider, value, AnimationDuration, AnimationEase);
        }
        
        private void HandleMoistUpdate(int value)
        {
            Tween.UISliderValue(moistSlider, value, AnimationDuration, AnimationEase);
        }
    }
}
