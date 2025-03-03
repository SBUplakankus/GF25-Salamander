// ============================================================================================
// CLASS: AbilitySliders
// ============================================================================================
// Description:
//   Updates the ability icons in the lower left of the screen on each usage
//
// Methods:
//   - HandleDashCooldown: When Dash has been used, reset the slider accurate to the cooldown time
//   - HandleSpitCooldown: When Spit has been used, reset the slider accurate to the cooldown time
//
// Related Classes:
//   - PlayerController: Listens in to the Player Spit and Dash events to know when to reset
// ============================================================================================

using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AbilitySliders : MonoBehaviour
    {
        [Header("Sliders")] 
        [SerializeField] private Slider dashSlider;
        [SerializeField] private Slider spitSlider;

        private void OnEnable()
        {
            PlayerController.OnPlayerSpit += HandleSpitCooldown;
            PlayerController.OnPlayerDash += HandleDashCooldown;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerDash -= HandleDashCooldown;
            PlayerController.OnPlayerSpit -= HandleSpitCooldown;
        }

        private void HandleDashCooldown(int duration)
        {
            dashSlider.value = 0;
            Tween.UISliderValue(dashSlider, dashSlider.maxValue, duration);
        }

        private void HandleSpitCooldown(int duration)
        {
            spitSlider.value = 0;
            Tween.UISliderValue(spitSlider, spitSlider.maxValue, duration);
        }
    }
}
