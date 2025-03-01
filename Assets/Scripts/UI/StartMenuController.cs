using System;
using System.Collections;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        [Header("Animation Settings")] 
        private const float AnimationDuration = 0.6f;
        private const float ButtonInterval = 0.25f;
        private const Ease AnimationEase = Ease.OutCubic;
        private const int CreditsPanelHideX = 1100;

        private void Start()
        {
            StartCoroutine(DisplayButtonsCoroutine());
        }

        private static void ShowPanel(RectTransform panel)
        {
            Tween.UIAnchoredPosition(panel, Vector2.zero, AnimationDuration, AnimationEase);
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
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private IEnumerator DisplayButtonsCoroutine()
        {
            yield return new WaitForSeconds(ButtonInterval);
            ShowPanel(titlePanel);
            yield return new WaitForSeconds(ButtonInterval * 3);
            ShowPanel(playPanel);
            yield return new WaitForSeconds(ButtonInterval);
            ShowPanel(creditsPanel);
            yield return new WaitForSeconds(ButtonInterval);
            ShowPanel(quitPanel);
        }
    }
}
