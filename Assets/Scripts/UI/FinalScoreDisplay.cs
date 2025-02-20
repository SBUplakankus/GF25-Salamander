using System;
using System.Collections;
using PrimeTween;
using Systems;
using TMPro;
using UnityEngine;

namespace UI
{
    public class FinalScoreDisplay : MonoBehaviour
    {
        [Header("Score Displays")] 
        [SerializeField] private RectTransform[] scoreBoxes;

        [Header("Score Texts")] 
        [SerializeField] private TMP_Text foodText;
        [SerializeField] private TMP_Text moistText;
        [SerializeField] private TMP_Text evtText;
        [SerializeField] private TMP_Text edText;
        [SerializeField] private TMP_Text ekText;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text finalScoreText;

        private const float AnimationDuration = 0.4f;
        private const Ease AnimationEase = Ease.OutCubic;
        private const float DisplayInterval = 0.35f;

        private void OnEnable()
        {
            StatsTracker.OnGameEnd += SetFinalScoreDisplay;
        }

        private void OnDisable()
        {
            StatsTracker.OnGameEnd -= SetFinalScoreDisplay;
        }

        private void SetFinalScoreDisplay(FinalStats stats)
        {
            foodText.text = stats.FoodScore.ToString();
            moistText.text = stats.MoistScore.ToString();
            evtText.text = stats.EnviroDamage.ToString();
            edText.text = stats.EnemyDamage.ToString();
            ekText.text = stats.EnemiesDefeated.ToString();
            timeText.text = stats.TimeSurvived.ToString();
        }

        public void StartDisplayAnimation()
        {
            StartCoroutine(ScoreDisplayCoroutine());
        }
        
        private IEnumerator ScoreDisplayCoroutine()
        {
            foreach (var box in scoreBoxes)
            {
                Tween.UIAnchoredPosition(box, Vector2.zero, AnimationDuration, AnimationEase, useUnscaledTime:true);
                yield return new WaitForSecondsRealtime(DisplayInterval);
            }
        }
    }
}
