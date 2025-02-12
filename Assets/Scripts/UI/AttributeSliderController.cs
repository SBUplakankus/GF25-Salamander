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
        }
        
        private void OnDisable()
        {
            PlayerAttributes.OnHealthLevelChanged -= HandleHealthUpdate;
            PlayerAttributes.OnHungerLevelChanged -= HandleHungerUpdate;
            PlayerAttributes.OnMoistLevelChanged -= HandleMoistUpdate;
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
