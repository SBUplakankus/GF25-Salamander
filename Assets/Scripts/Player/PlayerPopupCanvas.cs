using AI;
using PrimeTween;
using UnityEngine;

namespace Player
{
    public class PlayerPopupCanvas : MonoBehaviour
    {
        [Header("Notifications")] 
        [SerializeField] private RectTransform detected;
        [SerializeField] private RectTransform hungry;
        [SerializeField] private RectTransform dry;

        [Header("Animation")] 
        private const float AnimationDuration = 1.5f;
        private const Ease AnimationEase = Ease.OutBounce;

        [Header("Checks")] 
        private bool _hungryOpen;
        private bool _dryOpen;

        private void OnEnable()
        {
            PlayerAttributes.OnHungerLevelChanged += HandleHungerChange;
            PlayerAttributes.OnMoistLevelChanged += HandleMoistureChange;
            EnemyController.OnPlayerDetected += HandleEnemyDetection;
        }

        private void OnDisable()
        {
            PlayerAttributes.OnHungerLevelChanged -= HandleHungerChange;
            PlayerAttributes.OnMoistLevelChanged -= HandleMoistureChange;
            EnemyController.OnPlayerDetected -= HandleEnemyDetection;
        }

        private void HandleEnemyDetection(bool escaped)
        {
            if (escaped)
                HideNotification(detected);
            else
                ShowNotification(detected);
        }
        
        private void HandleHungerChange(int amount)
        {
            switch (amount)
            {
                case > 20 when _hungryOpen:
                    HideNotification(hungry);
                    break;
                case <= 20 when !_hungryOpen:
                    ShowNotification(hungry);
                    _dryOpen = true;
                    break;
            }
        }
        
        private void HandleMoistureChange(int amount)
        {
            switch (amount)
            {
                case > 20 when _dryOpen:
                    HideNotification(dry);
                    break;
                case <= 20 when !_dryOpen:
                    ShowNotification(dry);
                    _dryOpen = true;
                    break;
            }
        }
        
        private void HideNotification(RectTransform popup)
        {
            Tween.UISizeDelta(popup, Vector2.zero, AnimationDuration, AnimationEase);
        }
        
        private void ShowNotification(RectTransform popup)
        {
            Tween.UISizeDelta(popup, Vector2.one, AnimationDuration, AnimationEase);
        }
    }
}
