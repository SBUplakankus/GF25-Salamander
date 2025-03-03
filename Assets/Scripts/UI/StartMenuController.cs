using System;
using System.Collections;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class StartMenuController : MonoBehaviour
    {
        [Header("UI Elements")] 
        [SerializeField] private RectTransform titlePanel;
        [SerializeField] private RectTransform playPanel;
        [SerializeField] private RectTransform creditsPanel;
        [SerializeField] private RectTransform quitPanel;
        [SerializeField] private RectTransform creditsDisplayPanel;
        private bool _creditsOpen;

        [Header("Canvas Groups")] 
        [SerializeField] private CanvasGroup menu;
        [SerializeField] private CanvasGroup black;

        [Header("Animation Settings")] 
        private const float AnimationDuration = 0.6f;
        private const float ButtonInterval = 0.25f;
        private const Ease AnimationEase = Ease.OutCubic;
        private const int CreditsPanelHideX = 1100;

        private void Start()
        {
            StartCoroutine(DisplayButtonsCoroutine());
            FadeIn();
        }

        private static void ShowPanel(RectTransform panel)
        {
            Tween.UIAnchoredPosition(panel, Vector2.zero, AnimationDuration, AnimationEase);
        }

        private void FadeIn()
        {
            menu.alpha = 0;
            black.alpha = 1;
            menu.blocksRaycasts = false;
            Tween.Alpha(menu, 1, 1.5f).OnComplete(() => menu.blocksRaycasts = true);
            Tween.Alpha(black, 0, 1.5f);
        }

        private void FadeOut()
        {
            menu.blocksRaycasts = false;
            Tween.Alpha(menu, 0, 3f);
            Tween.Alpha(black, 1, 3f).OnComplete(() => SceneManager.LoadScene(1));
        }
        
        private void HideCreditsPanel()
        {
            var pos = creditsDisplayPanel.anchoredPosition;
            pos.x = CreditsPanelHideX;
            creditsDisplayPanel.anchoredPosition = pos;
            Tween.StopAll();
        }

        public void HandleCreditsPanel()
        {
            if (_creditsOpen)
                HideCreditsPanel();
            else 
                ShowPanel(creditsDisplayPanel);

            _creditsOpen = !_creditsOpen;
        }

        public void PlayGame()
        {
            FadeOut();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        
        /// <summary>
        /// Animation for the start of the game
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisplayButtonsCoroutine()
        {
            yield return new WaitForSeconds(ButtonInterval);
            ShowPanel(titlePanel);
            yield return new WaitForSeconds(ButtonInterval * 2);
            ShowPanel(playPanel);
            yield return new WaitForSeconds(ButtonInterval);
            ShowPanel(creditsPanel);
            yield return new WaitForSeconds(ButtonInterval);
            ShowPanel(quitPanel);
        }
    }
}
