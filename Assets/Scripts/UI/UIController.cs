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
        [SerializeField] private RectTransform pausePanel;
        [SerializeField] private RectTransform controlsPanel;
        [SerializeField] private RectTransform playerPanel;
        [SerializeField] private GameObject uiBlur;
        
        [Header("Animation Params")]
        private const int TutorialHideAmountY = 300;
        private const int PauseHideAmountX = -960;
        private const int ControlsHideAmountX = 900;
        private const int PlayerHideAmountX = -900;
        private const float AnimationDuration = 0.5f;
        private const Ease AnimationEase = Ease.OutCubic;

        [Header("Panel Status")] 
        private bool _pauseMenuOpen;

        private void Start()
        {
            _pauseMenuOpen = false;
            HidePausePanel();
            HideControlsPanel();
            DisableBlur();
        }

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Space))
            {
                HandlePauseMenuDisplay();
            }
        }
    
        
        
        #region Base Functions

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
        
        #endregion
        
        #region Hide Show Functions

        private void EnableBlur()
        {
            uiBlur.SetActive(true);
        }

        private void DisableBlur()
        {
            uiBlur.SetActive(false);
        }
        
        private void HideTutorialPanel()
        {
            HidePanel(1, tutorialPanel, TutorialHideAmountY);
        }

        private void ShowTutorialPanel(TutorialSO _)
        {
            ShowPanel(tutorialPanel);
        }

        private void HideControlsPanel()
        {
            HidePanel(0, controlsPanel, ControlsHideAmountX);
        }

        private void ShowControlsPanel()
        {
            ShowPanel(controlsPanel);
        }
        
        private void HidePausePanel()
        {
            HidePanel(0, pausePanel, PauseHideAmountX);
        }
        private void ShowPausePanel()
        {
            ShowPanel(pausePanel);
        }

        private void ShowPlayerPanel()
        {
            ShowPanel(playerPanel);
        }

        private void HidePlayerPanel()
        {
            HidePanel(0, playerPanel, PlayerHideAmountX);
        }
        
        #endregion

        #region Display Handlers

        private void HandlePauseMenuDisplay()
        {
            if (_pauseMenuOpen)
            {
                HideControlsPanel();
                HidePausePanel();
                DisableBlur();
                ShowPlayerPanel();
            }
            else
            {
                ShowControlsPanel();
                ShowPausePanel();
                EnableBlur();
                HidePlayerPanel();
            }

            _pauseMenuOpen = !_pauseMenuOpen;
        }
        
        #endregion
    }
}
