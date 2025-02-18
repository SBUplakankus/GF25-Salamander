using System;
using System.Collections;
using Player;
using PrimeTween;
using Scriptable_Objects;
using Systems;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [Header("UI Panels")] 
        [SerializeField] private RectTransform tutorialPanel;
        [SerializeField] private RectTransform tutorialSkipPanel;
        [SerializeField] private RectTransform pausePanel;
        [SerializeField] private RectTransform controlsPanel;
        [SerializeField] private RectTransform playerPanel;
        [SerializeField] private RectTransform gameOverPanel;
        [SerializeField] private GameObject uiBlur;
        
        [Header("Animation Params")]
        private const int TutorialHideAmountY = 300;
        private const int PauseHideAmountX = -960;
        private const int ControlsHideAmountX = 900;
        private const int PlayerHideAmountX = -900;
        private const int GameOverHideAmountY = -1000;
        private const int TutorialSkipHideX = -450;
        private const float AnimationDuration = 0.5f;
        private const Ease AnimationEase = Ease.OutCubic;

        [Header("Panel Status")] 
        private bool _pauseMenuOpen;

        private void Start()
        {
            _pauseMenuOpen = false;
            DisableBlur();
            UnPauseTime();
            StartCoroutine(DisplaySkipCoroutine());
        }

        private void OnEnable()
        {
            TutorialDisplay.OnHideTutorialDisplay += HideTutorialPanel;
            TutorialController.OnShowNextTutorial += ShowTutorialPanel;
            PlayerAttributes.OnGameOver += ShowGameOverPanel;
            TutorialController.OnTutorialSkip += HideTutorialPanel;
        }

        private void OnDisable()
        {
            TutorialDisplay.OnHideTutorialDisplay -= HideTutorialPanel;
            TutorialController.OnShowNextTutorial -= ShowTutorialPanel;
            PlayerAttributes.OnGameOver -= ShowGameOverPanel;
            TutorialController.OnTutorialSkip -= HideTutorialPanel;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
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
        
        private void ShowPanel(RectTransform panel, bool useTimeScale)
        {
            if (useTimeScale)
            {
                Tween.UIAnchoredPosition(panel, Vector2.zero, AnimationDuration, AnimationEase);
            }
            else
            {
                Tween.UIAnchoredPosition(panel, Vector2.zero, AnimationDuration, AnimationEase, useUnscaledTime: true);
            }
        }

        private void PauseTime()
        {
            Time.timeScale = 0;
        }

        private void UnPauseTime()
        {
            Time.timeScale = 1;
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
            ShowPanel(tutorialPanel, true);
        }

        private void HideControlsPanel()
        {
            HidePanel(0, controlsPanel, ControlsHideAmountX);
        }

        private void ShowControlsPanel()
        {
            ShowPanel(controlsPanel, false);
        }
        
        private void HidePausePanel()
        {
            HidePanel(0, pausePanel, PauseHideAmountX);
        }
        private void ShowPausePanel()
        {
            ShowPanel(pausePanel, false);
        }

        private void ShowPlayerPanel()
        {
            ShowPanel(playerPanel, true);
        }

        private void HidePlayerPanel()
        {
            HidePanel(0, playerPanel, PlayerHideAmountX);
        }

        private void ShowGameOverPanel()
        {
            ShowPanel(gameOverPanel, false);
            HidePlayerPanel();
            EnableBlur();
            PauseTime();
        }

        private void ShowTutorialSkipPanel()
        {
            ShowPanel(tutorialSkipPanel, true);
        }

        private void HideTutorialSkipPanel()
        {
            Tween.UIAnchoredPositionX(tutorialSkipPanel, TutorialSkipHideX, AnimationDuration, AnimationEase);
        }
        
        #endregion

        #region Display Handlers

        public void HandlePauseMenuDisplay()
        {
            if (_pauseMenuOpen)
            {
                HideControlsPanel();
                HidePausePanel();
                DisableBlur();
                ShowPlayerPanel();
                UnPauseTime();
            }
            else
            {
                ShowControlsPanel();
                ShowPausePanel();
                EnableBlur();
                HidePlayerPanel();
                PauseTime();
            }

            _pauseMenuOpen = !_pauseMenuOpen;
        }
        
        private IEnumerator DisplaySkipCoroutine()
        {
            ShowTutorialSkipPanel();
            yield return new WaitForSeconds(7);
            HideTutorialSkipPanel();
        }
        #endregion

        #region Button Functions

        public void QuitGame()
        {
            Application.Quit();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }

        #endregion
    }
}
