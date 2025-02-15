using System;
using PrimeTween;
using Scriptable_Objects;
using Systems;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [Header("UI Panels")] 
        [SerializeField] private RectTransform tutorialPanel;
        
        [Header("Animation Params")]
        private const int TutorialHideAmountY = 300;
        private const float AnimationDuration = 0.5f;
        private const Ease AnimationEase = Ease.OutCubic;

        private void OnEnable()
        {
            TutorialDisplay.OnHideTutorialDisplay += HideTutorialPanel;
            TutorialController.OnShowNextTutorial += ShowTutorialPanel;
        }

        private void OnDisable()
        {
            TutorialDisplay.OnHideTutorialDisplay -= HideTutorialPanel;
            TutorialController.OnShowNextTutorial -= ShowTutorialPanel;
        }

        private void HidePanel(int xy, RectTransform panel, int amount)
        {
            var pos = panel.anchoredPosition;
            
            if (xy == 0)
            {
                pos.x = amount;
            }
            else
            {
                pos.y = amount;
            }

            panel.anchoredPosition = pos;
        }

        private void ShowPanel(RectTransform panel)
        {
            Tween.UIAnchoredPosition(panel, Vector2.zero, AnimationDuration, AnimationEase);
        }

        private void HideTutorialPanel()
        {
            HidePanel(1, tutorialPanel, TutorialHideAmountY);
        }

        private void ShowTutorialPanel(TutorialSO _)
        {
            ShowPanel(tutorialPanel);
        }
    }
}
