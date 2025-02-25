using System;
using AI;
using PrimeTween;
using UnityEngine;

namespace Player
{
    public class PlayerPopupCanvas : MonoBehaviour
    {
        [Header("Notifications")] 
        [SerializeField] private Transform detected;
        [SerializeField] private RectTransform hungry;
        [SerializeField] private RectTransform dry;

        [Header("Animation")] 
        private const float AnimationDuration = 2f;
        private const Ease AnimationEase = Ease.OutBounce;

        [Header("Checks")] 
        private bool _hungryOpen;
        private bool _dryOpen;

        private void Start()
        {
            detected.localScale = Vector3.zero;
        }

        private void OnEnable()
        {
            //PlayerAttributes.OnHungerLevelChanged += HandleHungerChange;
            //PlayerAttributes.OnMoistLevelChanged += HandleMoistureChange;
            EnemyController.OnPlayerDetected += HandleEnemyDetection;
        }

        private void OnDisable()
        {
            //PlayerAttributes.OnHungerLevelChanged -= HandleHungerChange;
            //PlayerAttributes.OnMoistLevelChanged -= HandleMoistureChange;
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
            if (amount > 20 && _hungryOpen)
            {
                HideNotification(hungry);
                _hungryOpen = false;
                Debug.Log(amount + " " + _hungryOpen);
            }
            else if (amount <= 20 && !_hungryOpen)
            {
                ShowNotification(hungry);
                _hungryOpen = true;
                Debug.Log(amount + " " + _hungryOpen);
            }
        }
        
        private void HandleMoistureChange(int amount)
        {
            if (amount > 20 && _dryOpen)
            {
                HideNotification(dry);
                _dryOpen = false;
                Debug.Log(amount + " " + _dryOpen);
            }
            else if (amount <= 20 && !_dryOpen)
            {
                ShowNotification(dry);
                _dryOpen = true;
                Debug.Log(amount + " " + _dryOpen);
            }
        }
        
        private static void HideNotification(Transform popup)
        {
            Tween.Scale(popup, 0, AnimationDuration, AnimationEase);
        }
        
        private static void ShowNotification(Transform popup)
        {
            Tween.Scale(popup, 1, AnimationDuration, AnimationEase);
        }
    }
}
