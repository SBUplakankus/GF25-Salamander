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
        [SerializeField] private RectTransform abilitiesPanel;
        [SerializeField] private GameObject uiBlur;
        
        [Header("End Screen")]
        [SerializeField] private RectTransform sadPanel;
        [SerializeField] private RectTransform scorePanel;

        [Header("Canvas Groups")] 
        [SerializeField] private CanvasGroup tutorialGroup;
        [SerializeField] private CanvasGroup gameUi;
        [SerializeField] private CanvasGroup fadeToBlack;
        
        [Header("Animation Params")]
        private const int TutorialHideAmountY = 300;
        private const int PauseHideAmountX = -960;
        private const int ControlsHideAmountX = 1100;
        private const int PlayerHideAmountX = -900;
        private const int GameOverHideAmountY = -1100;
        private const int TutorialSkipHideX = -450;
        private const int AbilitiesHideX = 600;
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
            ShowPlayerAbilitiesPanel();
            ShowPlayerPanel();
            gameUi.alpha = 0;
            fadeToBlack.alpha = 1;
            Tween.Alpha(gameUi, 1, 2.5f);
            Tween.Alpha(fadeToBlack, 0, 2.5f);
        }

        private void OnEnable()
        {
            TutorialDisplay.OnHideTutorialDisplay += HideTutorialPanel;
            TutorialController.OnShowNextTutorial += ShowTutorialPanel;
            PlayerAttributes.OnGameOver += ShowGameOverPanel;
            TutorialController.OnTutorialSkip += HideTutorialPanel;
            GameManager.OnTimerExpiration += HandleTimerExpiration;
        }

        private void OnDisable()
        {
            TutorialDisplay.OnHideTutorialDisplay -= HideTutorialPanel;
            TutorialController.OnShowNextTutorial -= ShowTutorialPanel;
            PlayerAttributes.OnGameOver -= ShowGameOverPanel;
            TutorialController.OnTutorialSkip -= HideTutorialPanel;
            GameManager.OnTimerExpiration -= HandleTimerExpiration;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HandlePauseMenuDisplay();
            }
        }
        
        #region Base Functions

        private static void HidePanel(int xy, RectTransform panel, int amount)
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
        
        private static void ShowPanel(RectTransform panel, bool useTimeScale)
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

        private static void PauseTime()
        {
            Time.timeScale = 0;
        }

        private static void UnPauseTime()
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
            tutorialPanel.gameObject.SetActive(false);
            tutorialSkipPanel.gameObject.SetActive(false);
            HidePlayerPanel();
            HidePlayerAbilitiesPanel();
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

        public void SwapGameOverDisplay()
        {
            HidePanel(1, sadPanel, -1500);
            HidePanel(1,scorePanel,0);
        }

        private void HidePlayerAbilitiesPanel()
        {
            HidePanel(0, abilitiesPanel, AbilitiesHideX);
        }

        private void ShowPlayerAbilitiesPanel()
        {
            ShowPanel(abilitiesPanel, true);
        }
        
        #endregion

        #region Display Handlers

        public void HandlePauseMenuDisplay()
        {
            Tween.StopAll();
            if (_pauseMenuOpen)
            {
                HandleTutorialOpacity(true);
                HideControlsPanel();
                HidePausePanel();
                DisableBlur();
                ShowPlayerPanel();
                ShowPlayerAbilitiesPanel();
                UnPauseTime();
            }
            else
            {
                HandleTutorialOpacity(false);
                ShowControlsPanel();
                ShowPausePanel();
                EnableBlur();
                HidePlayerPanel();
                HidePlayerAbilitiesPanel();
                PauseTime();
            }

            _pauseMenuOpen = !_pauseMenuOpen;
        }

        private void HandleTimerExpiration()
        {
            StartCoroutine(EndGameCoroutine());
        }

        private void HandleTutorialOpacity(bool show)
        {
            tutorialGroup.alpha = show ? 1 : 0;
        }
        
        private IEnumerator DisplaySkipCoroutine()
        {
            ShowTutorialSkipPanel();
            yield return new WaitForSeconds(7);
            HideTutorialSkipPanel();
        }

        private IEnumerator EndGameCoroutine()
        {
            Tween.Alpha(gameUi, 0, 3f);
            Tween.Alpha(fadeToBlack, 1, 3f);
            yield return new WaitForSeconds(12f);
            SceneManager.LoadScene(2);
        }
        #endregion

        #region Button Functions

        public void QuitGame()
        {
            Application.Quit();
        }

        public void RestartGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(1);
        }

        #endregion
    }
}
